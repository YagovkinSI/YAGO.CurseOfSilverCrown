import * as React from 'react';
import { useAppDispatch, useAppSelector } from '../../store';
import { domainListActionCreators } from '../../store/DomainList';

const Domains: React.FC = () => {
    const state = useAppSelector(state => state.domainListReducer);
    const dispatch = useAppDispatch();

    React.useEffect(() => {
        if (!state.isLoading && state.domainPublicList == undefined)
            domainListActionCreators.getDomainList(dispatch, activeColumn)
    });

    const [activeColumn, setActiveColumn] = React.useState(7);

    const sortByColumn = (column: number) => {
        if (column == activeColumn)
            return;
        setActiveColumn(column);
        domainListActionCreators.getDomainList(dispatch, column)
    }

    return state.domainPublicList == null
        ? <p>Загрузка...</p>
        : (
            <React.Fragment>
                <h1>В разработке...</h1>
                <p>
                    Клиентская часть проекта переходит на технологию React, некоторые страницы пока недоступны.
                    Ожидаемое время завершения работ 1 июня.
                </p>
                <h1>Владение</h1>

                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(1)}>
                                    Владение
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(2)}>
                                    Войско
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(3)}>
                                    Казна
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(4)}>
                                    Развитие
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(5)}>
                                    Укрепления
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(6)}>
                                    Сюзерен
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(7)}>
                                    Кол-во васслов
                                </a>
                            </th>
                            <th>
                                <a style={{color:'blue', cursor:'pointer'}} onClick={() => sortByColumn(8)}>
                                    Игрок
                                </a>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {state.domainPublicList.map(domain => {
                            return <React.Fragment key={domain.id}>
                                <tr>
                                    <td>
                                        {domain.name}
                                    </td>
                                    <td>
                                        {domain.warriors}
                                    </td>
                                    <td>
                                        {domain.gold}
                                    </td>
                                    <td>
                                        {domain.investments}
                                    </td>
                                    <td>
                                        {domain.fortifications}
                                    </td>
                                    <td>
                                        {domain.suzerain?.name}
                                    </td>
                                    <td>
                                        {domain.vassalCount}
                                    </td>
                                    <td>
                                        {domain.user?.name}
                                    </td>
                                </tr >
                            </React.Fragment>
                        })}
                    </tbody>
                </table>
            </React.Fragment >
        );
}

export default Domains;
