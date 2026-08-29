import type { GameParameterValueStatus } from '../colonies/colony.types';

export type StatisticCode = 'Main' | 'MainMore';

export type StatisticCategory =
    | 'Info'
    | 'ActionPoints'
    | 'Solars'
    | 'SolarDelta'
    | 'Modules'
    | 'Mood'
    | 'Reforms'
    | 'Population'
    | 'PrivateCapital';

export interface StatisticField {
    category: StatisticCategory;
    label: string;
    value: string;
    status: GameParameterValueStatus;
    info: {
        name: string;
        imageName: string | null;
        description: string[];
    } | null;
    childrenCode: StatisticCode | null;
}

export interface Statistics {
    code: StatisticCode;
    title: string;
    fields: StatisticField[];
}
