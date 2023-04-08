import * as React from 'react';
import map1j from '../svg/map1';
import { SVGMap } from 'react-svg-map';
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import '../svg/index.css';

const GameMap: React.FC = () => {
    return (
        <div>
            <TransformWrapper
                initialScale={1}>
                <TransformComponent>
                    <SVGMap className='map' locationClassName='land' map={map1j}></SVGMap>
                </TransformComponent>
            </TransformWrapper>
        </div>
    );
};

export default GameMap;
