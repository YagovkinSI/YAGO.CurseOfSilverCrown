import { apiRequester } from "../shared/ApiRequester";
import { createMyDataMutation } from "./ApiResponse";
import type { ColonyParameterName } from "./ColonyParameterType";

export interface ColonyParameter {
    name: ColonyParameterName,
    value: number,
    isChanging: boolean
}

export interface Episode {
    id: number | undefined,
    slides: Slide[],
    choiceLabel: string | undefined,
    choice: Slide[] | undefined
}

export interface Slide {
    title: string,
    illustration: string,
    text: string[],
    parameters: ColonyParameter[]
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        createColony: createMyDataMutation<Episode, { name: string; presetType: ColonyPresetType; }>(
            'colony-actions/createColony', builder),

        runCycle: createMyDataMutation<Episode, {}>(
            'colony-actions/runCycle', builder),

        issueDecree: createMyDataMutation<Episode, { decreeId: number }>(
            'colony-actions/issueDecree', builder),

        deactivateColony: createMyDataMutation<Episode, {}>(
            'colony-actions/deactivateColony', builder, ["MyColony"])
    }),
});

export const {
    useRunCycleMutation,
    useIssueDecreeMutation,
    useCreateColonyMutation,
    useDeactivateColonyMutation
} = extendedApiSlice;