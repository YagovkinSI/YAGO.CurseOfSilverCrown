import * as React from 'react';
import { useAppDispatch, useAppSelector } from '../../store';
import { historyActionCreators } from '../../store/History';

const History: React.FC = () => {
    const state = useAppSelector(state => state.historyReducer);
    const dispatch = useAppDispatch();

    React.useEffect(() => {
        if (!state.isLoading && state.events == undefined)
            historyActionCreators.getEvents(dispatch)
    });

    const renderEvent = (event: string[]) => {
        let lineNum = 0;
        return event.map(line => {
            lineNum++;
            return (<p key={lineNum} style={{ marginBottom: 0 }}>{line}</p>)
        })
    }

    let eventNum = 0;
    return (
        <React.Fragment>
            <h1>В разработке...</h1>
            <p>
                Клиентская часть проекта переходит на технологию React, некоторые страницы пока недоступны.
                Ожидаемое время завершения работ 1 июня.
            </p>
            <div>
                <h4>
                    Важнейшие события
                </h4>
                {state.events == null
                    ? <p>'Загрузка...'</p>
                    : state.events.map(event => {
                        eventNum++;
                        return (
                            <React.Fragment key={eventNum}>
                                {renderEvent(event)}
                                < hr />
                            </React.Fragment>)
                    })}

            </div>
        </React.Fragment>
    );
}

export default History;
