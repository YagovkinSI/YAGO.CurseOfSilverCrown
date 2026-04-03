import type { ColonyParameter } from "./ColonyParameter"

export interface Slide {
    id: string,
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    footer?: string | undefined
}