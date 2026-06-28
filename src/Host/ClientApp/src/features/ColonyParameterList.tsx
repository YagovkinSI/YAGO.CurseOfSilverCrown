import { type ColonyParameter } from "../entities/ColonyParameter";
import StateList from "../shared/StateList";
import { GetStateItems } from "./GetColonyParameterList";

interface ColonyParameterListProps {
    items: ColonyParameter[]
}

const ColonyParameterList: React.FC<ColonyParameterListProps> = ({ items }) => {    

    return (
        <StateList items={GetStateItems(items)} />)        
}

export default ColonyParameterList