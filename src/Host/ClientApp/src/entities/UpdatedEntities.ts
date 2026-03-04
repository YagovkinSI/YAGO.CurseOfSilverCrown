import type { MyColony } from "./MyColony";
import type { MyCycle } from "./MyCycle";


export interface UpdatedEntities {
    myCycle: MyCycle | undefined;
    myColony: MyColony | undefined;
}
