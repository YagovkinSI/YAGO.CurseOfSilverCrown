import * as React from 'react';
import { useEffect } from 'react';
import SvgMap from './SvgMap';
import MapModal from './MapModal';
import { useAppDispatch, useAppSelector } from '../store';
import { mapActionCreators } from '../store/Map';
import { Spinner } from 'reactstrap';

import './Map.css';

const Map: React.FC = () => {
    const state = useAppSelector(state => state.mapReducer);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (!state.isLoading)
            mapActionCreators.getMap(dispatch)
    });

    return (
        <div>
            <h1>В разработке...</h1>
            <p>
                Клиентская часть проекта переходит на технологию React, некоторые страницы пока недоступны.
                Ожидаемое время завершения работ 1 июня.
            </p>
            <p style={{ textAlign: 'center' }}>Кликните на владение, чтобы получить по нему подробную информацию</p>
            {state.domainPublicList == undefined
                ?
                <Spinner>Загрузка...</Spinner>
                :
                <SvgMap domainPublicList={state.domainPublicList} />
            }
            <MapModal />
        </div>
    )
};

export default Map;
