import * as React from 'react';
import { Modal } from 'react-bootstrap';
import { useAppSelector } from '../store';

interface IMapModalProps {
    show: boolean,
    onClickClose: () => void
    domainId: number | undefined
}

const MapModal: React.FC<IMapModalProps> = (props) => {
    const state = useAppSelector(state => state.mapReducer);
    const domain = state.mapElements?.find(d => d.id == props.domainId);

    return (
        <Modal show={props.show} onHide={props.onClickClose}>
        <Modal.Header closeButton>
          <Modal.Title>{domain?.name ?? 'Владение неизвестно'}</Modal.Title>
        </Modal.Header>
        <Modal.Body>Информация о владении</Modal.Body>
      </Modal>
    )
};

export default MapModal;
