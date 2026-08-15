import type { ColonyParameter } from "./ColonyParameter";
import type { ColonyEvent } from "../events/ColonyEvent";

export interface ColonyPrivate {
    id: string,
    iserId: number,
    nextTurnstartAtUtc: string;
    name: string,
    colonyParameters: ColonyParameter[],
    quests: ColonyEvent[],
    actions: ColonyActions
}

export interface ColonyDetails {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}

export interface ColonyActions {
    reform: boolean,
    build: boolean
}