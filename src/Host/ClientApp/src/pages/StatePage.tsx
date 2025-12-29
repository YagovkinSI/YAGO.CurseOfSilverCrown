import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { WorkspacePremium } from '@mui/icons-material';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import StateList from '../shared/StateList';
import { StateItemPopulation, StateItemChallenges, StateItemShip, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';

const StatePage: React.FC = () => {
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.isAuthorized && myColonyResult.data!.data == undefined) {
            navigate('/createColony');
        }
    }, [navigate, myColonyResult]);

    const stats: StateItem[] = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${myColonyResult.data?.data?.name}`,
            color: '#9C27B0',
        },
        StateItemChallenges('Вызовы', `${myColonyResult.data?.data?.challenges}`),
        StateItemSolar('Солары', `${myColonyResult.data?.data?.solars} (${myColonyResult.data?.data?.solarsIncome}/ц)`),
        StateItemShip('Корабль', `Рассвет-782`),
        StateItemZones('Сектора', `${myColonyResult.data?.data?.zonesOccupied} / ${myColonyResult.data?.data?.zonesTotal}`),
        StateItemPopulation('Население', `${myColonyResult.data?.data?.population}`)
    ];

    const renderCard = () => {
        return (
            <YagoCard
                title={myColonyResult.data?.data?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                <StateList items={stats} /> 
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

export default StatePage