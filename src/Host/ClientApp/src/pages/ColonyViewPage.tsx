import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { ChildCare, Engineering, FlightTakeoff, Gavel, Groups, Home, Payments, SquareFoot, ThumbUp, TrendingUp, WorkspacePremium } from '@mui/icons-material';
import React from 'react';
import StateList from '../shared/StateList';
import { type StateItem, StateItemView } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import type { ColonyDetails } from '../entities/ColonyDetails';

const ColonyViewPage: React.FC = () => {
    //const { colonyIdString } = useParams();
    //const colonyId = Number(colonyIdString)
    //const colonyDetailsResult = useGetColonyDetailsQuery({colonyId});
    const data : ColonyDetails = {
        id: 0,
        iserId: 0,
        name: 'TestFront',
        solarsIncome: 0,
        challenges: 0,
        population: 0,
        zonesOccupied: 0
    }

    const isLoading = false //colonyDetailsResult.isLoading;
    const error = undefined //colonyDetailsResult.error;

    const navigate = useNavigate();

    const stats: StateItem[] = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${data?.name}`,
            color: '#9C27B0',
            url: '/state'
        },
        StateItemView('ВВП', `${data?.solarsIncome} тыс.солар`, TrendingUp),
        StateItemView('Население', `${data?.population} чел.`, Groups),
        StateItemView('Площадь', `${data?.zonesOccupied} м²`, SquareFoot),
        StateItemView('Одобрение', `${15}%`, ThumbUp),
        StateItemView('Преступления', `${data?.challenges} в год`, Gavel),
        StateItemView('Покинули колонию', `${data?.population} за 5 лет`, FlightTakeoff),
        StateItemView('Сред. зарплата', `${0.95} солар/год`, Payments),
        StateItemView('Жильё', `${data?.challenges} м²/чел`, Home),
        StateItemView('Рождаемость', `${0.3} ребёнка`, ChildCare),
        StateItemView('Рудокопы', `${78}% населения`, Engineering),
    ];

    const renderCard = () => {
        return (
            <YagoCard
                title={data?.name ?? '-'}
                image={undefined}
            >
                <StateList items={stats} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard()}
        </>
    )
}

export default ColonyViewPage