import type { ColonyParameter } from "../colonies/ColonyParameter";

export interface EventResultSlide {
    title: string,
    text: string[],
    parameters: ColonyParameter[],
    imageName?: string,
}