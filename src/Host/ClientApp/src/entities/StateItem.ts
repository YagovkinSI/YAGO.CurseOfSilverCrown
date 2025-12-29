import { AttachMoney, People, RocketLaunch, ViewModule, Whatshot } from "@mui/icons-material";
import type { SvgIconTypeMap } from "@mui/material";
import type { OverridableComponent } from "@mui/material/OverridableComponent";

export interface StateItem {
    color: string,
    icon: OverridableComponent<SvgIconTypeMap<Record<string, unknown>, "svg">> & { muiName: string; },
    label: string,
    value: string | number,
    url?: string | undefined
}

export const StateItemSolar = (label: string, value: string | number): StateItem => {
    return {
        icon: AttachMoney,
        label,
        value,
        color: '#FFD700'
    }
}

export const StateItemChallenges = (label: string, value: string | number): StateItem => {
    return {
        icon: Whatshot,
        label,
        value,
        color: '#DC2626'
    }
}

export const StateItemPopulation = (label: string, value: string | number): StateItem => {
    return {
        icon: People,
        label,
        value,
        color: '#81C784'
    }
}

export const StateItemShip = (label: string, value: string | number): StateItem => {
    return {
        icon: RocketLaunch,
        label,
        value,
        color: '#FF8A65',
        url: '/ship'
    }
}

export const StateItemZones = (label: string, value: string | number): StateItem => {
    return {
        icon: ViewModule,
        label,
        value,
        color: '#757575'
    }
}

export const StateItemView = (
    label: string, 
    value: string | number,
    icon: OverridableComponent<SvgIconTypeMap<Record<string, unknown>, "svg">> & { muiName: string; }): StateItem => {
    return {
        icon,
        label,
        value,
        color: '#bfe7faff'
    }
}