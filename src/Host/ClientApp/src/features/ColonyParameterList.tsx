import { type ColonyParameter } from "../entities/ColonyParameter";
import ColonyParameterRowList from "../shared/ColonyParameterRowList";
import { GetStateItems } from "./GetColonyParameterList";

interface ColonyParameterListProps {
    items: ColonyParameter[];
}

const ColonyParameterList: React.FC<ColonyParameterListProps> = ({ items }) => {

    const renderStateList = () => (
        <ColonyParameterRowList items={GetStateItems(items)} />
    );

    return renderStateList();
};

export default ColonyParameterList;