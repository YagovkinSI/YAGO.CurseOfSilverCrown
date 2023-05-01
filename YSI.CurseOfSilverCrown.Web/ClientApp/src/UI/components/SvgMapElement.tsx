import * as React from 'react';
import IMapElement from '../../apiModels/mapElement';

interface ISvgMapElementProps {
    mapElement: IMapElement,
    path: string,
    onClickDomain: (id: number) => void
}

const SvgMapElement: React.FC<ISvgMapElementProps> = (props) => {
    const onClick = () => {
        props.onClickDomain(props.mapElement.id)
    }

    return (
        <path
            key={props.mapElement.id}
            id={props.mapElement.id.toString()}
            className="feoda"
            d={props.path}
            style={{ fill: props.mapElement.colorKingdom }}
            onClick={onClick}
        />
    )
};

export default SvgMapElement;
