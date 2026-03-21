import type { Slide } from "./Slide";


export interface Episode {
    id: string | undefined;
    slides: Slide[];
    choiceLabel: string | undefined;
    choice: Slide[] | undefined;
    isCycleCompleted: boolean;
}
