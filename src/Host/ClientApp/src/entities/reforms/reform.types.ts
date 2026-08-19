import type { ColonyParameter } from "../colonies/colony.types";
import type { SlideButton } from "../events/colonyEvent.types";

export interface ReformDetails {
    code: string,
    name: string,
    image: string,
    parameters: ColonyParameter[],
    requirements: ColonyParameter[],
    description: string[],
    button: SlideButton
}