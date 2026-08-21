import type { IconType } from "../../../shared/ui/icons/GameIcon";

export interface GameRequirement {
    iconType: IconType;
    label: string;
    value: string;
    isMet: boolean;
    isLabelFirst: boolean;
    url?: string;
    infoUrl?: string;
}