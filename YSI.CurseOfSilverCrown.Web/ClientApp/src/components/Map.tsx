import * as React from 'react';
import SvgMap from './SvgMap';

import './Map.css';
import MapModal from './MapModal';

const Map: React.FC = () => {
    return (
        <div>
            <h1>В разработке...</h1>
            <p>
                Клиентская часть проекта переходит на технологию React, некоторые страницы пока недоступны.
                Ожидаемое время завершения работ 1 июня.
            </p>
            <p style={{textAlign: 'center'}}>Кликните на владение, чтобы получить по нему подробную информацию</p>
            <SvgMap />
            <MapModal />
        </div>
    )
};

export default Map;
