import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("uint8")
    loss = 0;

    @type("int8")
    maxHP = 0;

    @type("int8")
    curHP = 0;
    
    @type("number")
    speed = 0;

    @type("number")
    pX = Math.floor(Math.random() * 50) - 25;

    @type("number")
    pY = 0;

    @type("number")
    pZ = Math.floor(Math.random() * 50) - 25;

    @type("number")
    vX = 0;
    
    @type("number")
    vY = 0;

    @type("number")
    vZ = 0;

    @type("number")
    rX = 0;
    
    @type("number")
    rY = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any) {
        const player = new Player();
        player.maxHP = data.hp;
        player.curHP = data.hp;
        player.speed = data.speed;

        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {
            const player = this.players.get(sessionId);

            player.pX = data.pX;
            player.pY = data.pY;
            player.pZ = data.pZ;

            player.vX = data.vX;
            player.vY = data.vY;
            player.vZ = data.vZ;

            player.rX = data.rX;
            player.rY = data.rY;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) => {
            this.broadcast("enShoot", data, {except : client});
        });

        this.onMessage("sit", (client, data) => {
            this.broadcast("enSit", data, {except : client});
        });

        this.onMessage("dmg", (client, data) => {
            const sessionId = data.id;
            const player = this.state.players.get(sessionId);
            const hp = player.curHP - data.dmg;

            if(hp > 0)
            {
                player.curHP = hp;
                return;
            }

            player.loss++;
            player.curHP = player.maxHP;

            this.clients.forEach(function(client){
                if(client.id == sessionId){
                    const x = Math.floor(Math.random() * 50) - 25;
                    const z = Math.floor(Math.random() * 50) - 25;
                    const message = JSON.stringify({x,z});
                    client.send("respawn", message);
                }
            })
        });

        this.onMessage("gun", (client, data) => {
            this.broadcast("enGun", data, {except: client});
        })

        this.onMessage("chat", (client, data) => {
            this.broadcast("mes", data, {except : client});
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
       // if(this.clients.length > 1) this.lock();
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
