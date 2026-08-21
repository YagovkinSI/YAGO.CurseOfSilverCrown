import type { IconType } from "../../../shared/ui/icons/GameIcon";

export type EffectColor = 'Negative' | 'Neutral' | 'Positive';

export interface GameVisibleEffect {
    iconType: IconType;
    label: string;
    value: string;
    color: EffectColor;
    url?: string;
    infoUrl?: string;
}