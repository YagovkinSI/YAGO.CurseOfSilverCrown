import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import StateList from '../shared/StateList';

const ShipPage: React.FC = () => {
    const navigate = useNavigate();

    const ship = {
        name: 'Рассвет-782',
        imageName: 'ship_1',
        cost: 8000,
        zones: 10000,
        comment: '«Стандартный корабль-город для начинающих правителей. Скромный, но функциональный.»' 
    }

    const stats: StateItem[] = [
        StateItemSolar('Цена', `${ship.cost}`),
        StateItemZones('Сектора', `${ship.zones}`)
    ];

    const isLoading = false;
    const error = undefined;

    const renderCard = () => {
        return (
            <YagoCard
                title={ship.name}
                image={`/assets/images/pictures/${ship.imageName ?? 'home'}.jpg`}
            >
                <StateList items={stats} />
                <Typography sx={{ mt: 1 }} textAlign="center" className='text-mutted' gutterBottom>
                    {ship.comment}
                </Typography>
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

export default ShipPage