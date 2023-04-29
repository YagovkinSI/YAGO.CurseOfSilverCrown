import * as React from 'react';
import SvgMapImage from './SvgMapImage';
import getFeods from './Feods';


const SvgMap: React.FC = () => {
    const feods = getFeods();

    return (
        <svg version="1.1" viewBox="0 0 1920 2887" xmlns="http://www.w3.org/2000/svg">
            <g transform="translate(0 .075438)" style={{ strokeWidth: 1.0016 }}>
                <SvgMapImage />
                {feods.map(feod => {
                    return <path
                        key={feod.id}
                        id={feod.id.toString()}
                        className="feoda"
                        d={feod.path}
                        style={{fill:'rgb(30, 150, 40, 0.5)'}}
                    />
                })}
            </g>
        </svg>
    )
};

export default SvgMap;
