import type { ColonyParameter } from "./ColonyParameter";

export interface EventResultSlide {
    title: string,
    text: string[],
    parameters: ColonyParameter[],
    imageName?: string,
}