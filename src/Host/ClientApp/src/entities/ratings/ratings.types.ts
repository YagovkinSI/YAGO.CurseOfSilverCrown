import type { StatisticField } from '../statistics/statistics.types';

export type RatingCode =
    | 'population'
    | 'laws'
    | 'mood'
    | 'budget'
    | 'area'
    | 'week';

export type RatingsResponse = StatisticField[];
