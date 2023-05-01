import * as React from 'react';
import { useEffect } from 'react';
import SvgMap from '../components/SvgMap';
import MapModal from '../components/MapModal';
import { useAppDispatch, useAppSelector } from '../../store';
import { mapActionCreators } from '../../store/Map';
import { Spinner } from 'reactstrap';

import './Map.css';

interface IModalSettings {
    show: boolean,
    activeDomainId: number | undefined
}

const Map: React.FC = () => {
    const state = useAppSelector(state => state.mapReducer);
    const dispatch = useAppDispatch();

    const defaultModalSettings: IModalSettings = {
        activeDomainId: undefined,
        show: false
    }
    const [modalSettings, setModalSettings] = React.useState(defaultModalSettings);

    useEffect(() => {
        if (!state.isLoading && state.mapElements == undefined)
            mapActionCreators.getMap(dispatch)
    });

    const onClickDomain = (id: number) => {
        const domain = state.mapElements?.find(d => d.id == id);
        if (domain == undefined)
            return;
        
        const modalSettings: IModalSettings = {
            activeDomainId: id,
            show: true
        }
        setModalSettings(modalSettings);
    } 

    return (
        <div>
            <p style={{ textAlign: 'center' }}>Кликните на владение, чтобы получить по нему подробную информацию</p>
            {state.mapElements == undefined
                ?
                <Spinner>Загрузка...</Spinner>
                :
                <SvgMap mapElements={state.mapElements} onClickDomain={onClickDomain} />
            }
            <MapModal 
                domainId={modalSettings.activeDomainId} 
                show={modalSettings.show} 
                onClickClose={() => setModalSettings({ ...modalSettings, show: false })}/>
        </div>
    )
};

export default Map;
