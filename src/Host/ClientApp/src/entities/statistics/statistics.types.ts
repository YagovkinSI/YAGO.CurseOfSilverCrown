import type { GameParameterValueStatus } from '../colonies/colony.types';
import type { DisplayInfo } from '../common/common.types';

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
    info: DisplayInfo | undefined;
    childrenCode: string | undefined;
}

export interface Statistics {
    code: string;
    title: string;
    fields: StatisticField[];
}
