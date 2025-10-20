import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { StateItemPopulation, StateItemReputation, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import StateList from '../shared/StateList';
import type { Slide } from '../entities/Slide';
import SlideCard from '../features/SlideCard';

const BuildingPage: React.FC = () => {
    const navigate = useNavigate();

    const [showSlide, setShowSlide] = useState<boolean>(false);

    const building = {
        id: 2,
        name: 'ЖС "Экноном"',
        imageName: 'pragmatist',
        cost: 1250,
        zones: 1000,
        solarsIncome: 100,
        population: 100,
        description: [
            'Жилищный сектор "Экноном".',
            'Сбалансированный подход. Вы обеспечите приемлемый комфорт для эффективной работы, найдя золотую середину между благополучием колонии и прибылью.']
    }

    const stats: StateItem[] = [
        StateItemSolar('Цена', `${building.cost}`),
        StateItemZones('Зоны', `${building.zones} м²`),
        StateItemSolar('Доход', `+${building.solarsIncome}/ц`),
        StateItemReputation('Репутация', `+0`),
        StateItemPopulation('Население', `+${building.population} чел.`),
    ];

    const isLoading = false;
    const error = undefined;

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
        return (
            <YagoCard
                title={building.name}
                image={`/assets/images/pictures/${building.imageName ?? 'home'}.jpg`}
            >
                <StateList items={stats} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
                <YagoButton variant="contained" onClick={() => navigate(-1)} text={'Купить'} isDisabled={true} />
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