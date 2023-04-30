import * as React from 'react';
import IDomainPublic from '../apiModels/domainPublic';

interface ISvgMapElementProps {
    domainPublic: IDomainPublic,
    path: string,
    onClickDomain: (id: number) => void
}

const SvgMapElement: React.FC<ISvgMapElementProps> = (props) => {
    const onClick = () => {
        props.onClickDomain(props.domainPublic.id)
    }

    return (
        <path
            key={props.domainPublic.id}
            id={props.domainPublic.id.toString()}
            className="feoda"
            d={props.path}
            style={{ fill: props.domainPublic.colorKingdom }}
            onClick={onClick}
        />
    )
};

export default SvgMapElement;
