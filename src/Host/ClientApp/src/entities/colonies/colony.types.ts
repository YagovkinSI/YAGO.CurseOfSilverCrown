import type { ColonyParameter } from "./ColonyParameter";
import type { ColonyEvent } from "../events/ColonyEvent";

export interface ColonyPrivate {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[],
    quests: ColonyEvent[],
    newColonyAvailable: boolean,
    solars: number,
    zonesAvailable: number
}

export interface ColonyDetails {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}