import type { GameRequirement } from "../common/gameRequirements/gameRequirement.types";
import type { GameVisibleEffect } from "../common/gameVisibleEffects/gameVisibleEffect.types";
import type { SlideButton } from "../events/colonyEvent.types";

export interface ReformDetails {
    code: string,
    name: string,
    image: string,
    visibleEffects: GameVisibleEffect[],
    requirements: GameRequirement[],
    description: string[],
    button: SlideButton
}