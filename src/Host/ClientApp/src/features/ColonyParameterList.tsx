import type { ColonyParameter } from "../entities/colonies/colony.types";
import ColonyParameterRowList from "../entities/colonies/ColonyParameterRowList";
import { GetStateItems } from "./GetColonyParameterList";

interface ColonyParameterListProps {
    items: ColonyParameter[];
    dense?: boolean,
}

const ColonyParameterList: React.FC<ColonyParameterListProps> = ({ items, dense }) => {

    const renderStateList = () => (
        <ColonyParameterRowList items={GetStateItems(items)} dense={dense} />
    );

    return renderStateList();
};

export default ColonyParameterList;