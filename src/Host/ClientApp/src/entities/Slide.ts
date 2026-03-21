import type { ColonyParameter } from "./ColonyParameter"

export interface Slide {
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    footer?: string | undefined
}