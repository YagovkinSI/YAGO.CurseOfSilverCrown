import { AttachMoney, ViewModule } from "@mui/icons-material";
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

export const StateItemZones = (label: string, value: string | number): StateItem => {
    return {
        icon: ViewModule,
        label,
        value,
        color: '#757575'
    }
}