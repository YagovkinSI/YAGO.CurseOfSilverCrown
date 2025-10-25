import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { StateItemPopulation, StateItemReputation, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import StateList from '../shared/StateList';
import type { Slide } from '../entities/Slide';
import SlideCard from '../features/SlideCard';
import { useBuyBuildingMutation, useGetMyColonyQuery } from '../entities/MyColony';
import isErrorWithStatus from '../shared/ErrorHandler';

const BuildingPage: React.FC = () => {
    const navigate = useNavigate();

    const myColonyResult = useGetMyColonyQuery();
    const [useBuyBuilding, useBuyBuildingResult] = useBuyBuildingMutation();

    const [showSlide, setShowSlide] = useState<boolean>(false);

    const building = {
        id: 2,
        name: 'ЖС "Экноном"',
        imageName: 'pragmatist',
        cost: 1250,
        zones: 25,
        solarsIncome: 120,
        population: 200,
        description: [
            'Жилищный сектор "Экноном".',
            'Сбалансированный подход. Вы обеспечите приемлемый комфорт для эффективной работы, найдя золотую середину между благополучием колонии и прибылью.']
    }

    const isLoading = myColonyResult.isLoading || useBuyBuildingResult.isLoading;
    const error = myColonyResult.error ?? useBuyBuildingResult.error;

    useEffect(() => {
        if (error == undefined && isErrorWithStatus(error, 401))
            navigate('/registration');        
    }, [error]);

    const buyBuilding = async () => {
        await useBuyBuilding({ buildingId: building.id }).unwrap();
        navigate('/me/colony');
    }

    const stats: StateItem[] = [
        StateItemSolar('Цена', `${building.cost}`),
        StateItemZones('Сектора', `${building.zones}`),
        StateItemSolar('Доход', `+${building.solarsIncome}/ц`),
        StateItemReputation('Репутация', `+0`),
        StateItemPopulation('Население', `+${building.population} чел.`),
    ];

    const renderSlideCard = () => {
        const slide: Slide = {
            id: building.id,
            title: building.name,
            imageName: building.imageName,
            text: building.description,
            footer: undefined
        };

        return (
            <SlideCard slide={slide} closeAction={() => setShowSlide(false)} />
        )
    }

    const renderCard = () => {
        const isActive = myColonyResult.data?.isAuthorized
            && myColonyResult.data.data != undefined
            && myColonyResult.data.data.solars > building.cost
            && myColonyResult.data.data.zonesTotal - myColonyResult.data.data.zonesOccupied >= building.zones

        return (
            <YagoCard
                title={building.name}
                image={`/assets/images/pictures/${building.imageName ?? 'home'}.jpg`}
            >
                <StateList items={stats} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
                <YagoButton variant="contained" onClick={buyBuilding} text={'Купить'} isDisabled={!isActive} />
                <YagoButton onClick={() => setShowSlide(true)} text={'Описание'} />
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
                    : showSlide
                        ? renderSlideCard()
                        : renderCard()}
        </>
    )
}

export default BuildingPage