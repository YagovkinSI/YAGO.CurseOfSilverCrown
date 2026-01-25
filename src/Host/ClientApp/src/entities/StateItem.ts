import { AttachMoney, Balance, People, RocketLaunch, ViewModule } from "@mui/icons-material";
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

export const StateItemGavernorType = (label: string, value: string | number): StateItem => {
    let stringValue = "Неопределен";
    if (typeof value === 'string')
        stringValue = value
    else
    {
        const valueInt = Math.max(1, Math.min(3, Math.round(value))); 
        switch (valueInt) {
        case 1:
            stringValue = "Гуманист"
            break
        case 2:
            stringValue = "Центрист"
            break
        case 3:
            stringValue = "Капиталист"
            break
        }
    }
    
    return {
        icon: Balance,
        label,
        value: stringValue,
        color: '#4FC3F7'
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
    let stringValue = "Неопределен";
    let shipId = 0;
    if (typeof value === 'string')
        stringValue = value
    else
    {
        const shipId = Math.max(1, Math.min(2, Math.round(value))); 
        switch (shipId) {
        case 1:
            stringValue = "Рассвет-782"
            break
        case 2:
            stringValue = "Резолют-206"
            break
        }
    }

    return {
        icon: RocketLaunch,
        label,
        value: stringValue,
        color: '#FF8A65',
        url: shipId == 0 ? undefined : `/wiki/ship/${shipId}}`
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