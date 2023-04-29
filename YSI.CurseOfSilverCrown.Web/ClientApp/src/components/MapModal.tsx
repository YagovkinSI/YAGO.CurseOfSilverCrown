import * as React from 'react';

const MapModal: React.FC = () => {
    return (
        <div id="modDialog" className="modal fade">
            <div id="dialogContent" className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 id="infoDomainName">Владение</h4>
                        <button className="close" data-dismiss="modal" area-hidden="true">X</button>
                    </div>
                    <div className="modal-body">
                        <dl id="infoLines" className="dl-horizontal">
                            <dt>Информация</dt>
                        </dl>
                    </div>
                </div>
            </div>
        </div>
    )
};

export default MapModal;
