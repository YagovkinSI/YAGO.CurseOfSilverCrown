import * as React from 'react';
import { Modal } from 'react-bootstrap';
import { useAppDispatch, useAppSelector } from '../../store';
import { activeDomainActionCreators } from '../../store/ActiveDomain';

interface IMapModalProps {
  show: boolean,
  onClickClose: () => void
  domainId: number | undefined
}

const MapModal: React.FC<IMapModalProps> = (props) => {
  const state = useAppSelector(state => state.activeDomainReducer);
  const dispatch = useAppDispatch();

  const domain = state.activeDomain != undefined && state.activeDomain.id == props.domainId
    ? state.activeDomain
    : undefined;

  React.useEffect(() => {
    if (!state.isLoading && props.domainId != undefined && domain == undefined)
      activeDomainActionCreators.getDomainPublic(dispatch, props.domainId)
  })

  let lineIndex = 0;
  return (
    <Modal show={props.show} onHide={props.onClickClose}>
      <Modal.Header closeButton>
        <Modal.Title>{state.activeDomain?.name ?? 'Загрузка...'}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {domain != undefined
          ? domain.info.map(infoLine => {
            lineIndex++;
            return infoLine == '<hr>'
              ? <hr key={lineIndex} />
              : <p key={lineIndex} style={{ margin: 0, padding: 0 }}>{infoLine}</p>
          })
          : 'Загрузка...'
        }
      </Modal.Body>
    </Modal>
  )
};

export default MapModal;
