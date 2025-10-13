import React, { useState } from 'react';
import { Button, CircularProgress } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { type MyState } from '../entities/MyState';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import YagoCard from '../shared/YagoCard';
import YagoButton from '../shared/YagoButton';
import TextMain from '../shared/TextMain';
import StateList from '../shared/StateList';
import { StateItemPopulation, StateItemReputation, StateItemShip, StateItemSolar, StateItemZones } from '../entities/StateItem';
import YagoCardSContentSelection from '../shared/YagoCardSContentSelection';
import YagoCardSContentInputField from '../shared/YagoCardSContentInputField';

interface PresetOption {
    label: string;
    image: string;
    description: string;
    comment: string;
    reputation: number,
    income: number,
    population: number
}

const CreateClolonyPage: React.FC = () => {
    const navigate = useNavigate();

    const error: FetchBaseQueryError | SerializedError | undefined = undefined;
    const isLoading = false;

    const isSending = false;
    const [step, setStep] = useState<number>(0);
    const [name, setName] = useState('');
    const [nameError, setNameError] = useState('');

    const [colonyData, setColonyData] = useState<Partial<MyState>>({
        id: 0,
        name: '-',
        iserId: 0,
        reputation: 400,
        solars: 10000,
        income: 50,
        population: 100,
        ship: 'Расвет-782',
        freeZones: 50
    });

    const presets: PresetOption[] = [
        {
            label: 'Гуманист',
            image: 'humanist',
            description: 'Просторные жилые зоны и развитая социальная инфраструктура. Ваши люди будут счастливы и лояльны, что обеспечит долгосрочную стабильность.',
            comment: '«Благополучие жителей — главный приоритет.»',
            reputation: 400,
            income: +50,
            population: 160,
        },
        {
            label: 'Прагматик',
            image: 'pragmatist',
            description: 'Сбалансированный подход. Вы обеспечите приемлемый комфорт для эффективной работы, найдя золотую середину между благополучием колонии и прибылью.',
            comment: '«Стабильность и умеренный рост.»',
            reputation: 0,
            income: +60,
            population: 200,
        },
        {
            label: 'Диктатор',
            image: 'dictator',
            description: 'Максимальная эффективность и прибыль любой ценой. Вы втиснете больше рабочих в меньший объём, пожертвовав комфортом ради быстрого стартового рывка.',
            comment: '«Цель оправдывает средства.»',
            reputation: -400,
            income: +70,
            population: 240,
        }
    ];

    const getCurrentPresetIndex = () =>
        presets.findIndex(r => r.income === colonyData.income);

    const handleNextPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const nextIndex = (currentIndex + 1) % presets.length;
        setColonyData({
            ...colonyData,
            reputation: presets[nextIndex].reputation,
            income: presets[nextIndex].income,
        });
    };

    const handlePrevPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const prevIndex = (currentIndex - 1 + presets.length) % presets.length;
        setColonyData({
            ...colonyData,
            reputation: presets[prevIndex].reputation,
            income: presets[prevIndex].income,
        });
    };

    const handleSaveColony = async () => {
        try {
            //await createColony({ colony: colonyData as MyState }).unwrap();
            navigate('/state');
        } catch (error) {
            console.error('Failed to create colony:', error);
        }
    };

    const validateName = (value: string): boolean => {
        const regex = /^[a-zA-Zа-яА-Я0-9 -]{3,16}$/;
        if (!regex.test(value)) {
            setNameError('Название должно содержать 3-16 символов (буквы, цифры, пробел и "-")');
            return false;
        }
        setNameError('');
        return true;
    };

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setName(value);
        if (value.length > 0) {
            validateName(value);
        } else {
            setNameError('');
        }
    };

    const handleSave = () => {
        if (validateName(name)) {
            setColonyData({ ...colonyData, name });
            handleSaveColony();
        }
    };

    const renderLoreCard = () => {
        return (
            <YagoCard
                title='Новая Эра'
                image={`/assets/images/pictures/future_station.jpg`}
            >
                <TextMain textArray={[
                    'Середина XXV века. Национальные государства давно пали под натиском корпораций.',
                    'Миром правят частные компании и владельцы кораблей-городов. Их богатство построено на труде миллионов обездоленных, что готовы годами жить в тесноте и, рискуя жизнью, добывать руду на астероидах. Всё ради призрачного шанса на лучшую долю для своих детей.'
                ]} />
                <YagoButton onClick={() => setStep(step + 1)} text={'Далее'} isDisabled={false} />
            </YagoCard>
        )
    }

    const renderShipCard = () => {
        return (
            <YagoCard
                title='Ваш Актив'
                image={`/assets/images/pictures/ship_1.jpg`}
            >
                <StateList items={[StateItemShip('Корабль', `Рассвет-782`)]} sx={{ mb: '8px' }} />
                <TextMain textArray={[
                    'Теперь и вы обладетль собственного корабля. Серийный, неказистый, но полностью функциональный корабль-город с добывающим комплексом.',
                    'Его цеха готовы к переработке льда и руды в Поясе Астероидов. Но вам нужны люди. Вам нужна колония.'
                ]} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <YagoButton onClick={() => setStep(step + 1)} text={'Далее'} isDisabled={false} />
            </YagoCard>
        )
    }

    const renderHangarCard = () => {
        return (
            <YagoCard
                title='Чистый Лист'
                image={`/assets/images/pictures/empty_hangar.jpg`}
            >
                <StateList items={[StateItemZones('Зоны', `0 / 10 000 м²`)]} sx={{ mb: '8px' }} />
                <TextMain textArray={[
                    '10 000 квадратных метров пустого пространства. Здесь будут жить те, чьим трудом выстроится ваше богатство.',
                    'Вам предстоит решить: в каких условиях они будут существовать, какие законы будут ими управлять и какое общество вы создадите на этом клочке стали, затерянном в пустоте космоса.'
                ]} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <YagoButton onClick={() => setStep(step + 1)} text={'Далее'} isDisabled={false} />
            </YagoCard>
        )
    }

    const renderPresetsCard = () => {
        const currentPreset = presets.find(r => r.income === colonyData.income)!;
        const image = currentPreset.image;

        return (
            <YagoCard
                title='Выбор Пути'
                image={`/assets/images/pictures/${image ?? 'home'}.jpg`}
            >
                <TextMain textArray={['Выберите стиль правления для вашей колонии']} sx={{ textAlign: 'center' }} />
                <YagoCardSContentSelection handlePrev={handlePrevPreset} label={currentPreset.label} handleNext={handleNextPreset} />
                <StateList
                    items={[
                        StateItemSolar('Солары', `1 000 (${currentPreset.income} / ч.)`),
                        StateItemReputation('Репутация', `${currentPreset.reputation}`),
                        StateItemZones('Зоны', `4 000 / 10 000 м²`),
                        StateItemPopulation('Население', `${currentPreset.population} чел.`),
                    ]}
                    sx={{ mb: '8px' }} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <Button variant="contained" onClick={() => setStep(step + 1)}>Выбрать</Button>
                {/*<TextMain textArray={[currentPreset.description]} />
                <TextFooterComment>{currentPreset.comment}</TextFooterComment>*/}
            </YagoCard>
        )
    }

    const renderNameCard = () => {
        return (
            <YagoCard
                title='Название колонии'
                image={`/assets/images/pictures/register_colony.jpg`}
            >
                <TextMain textArray={[
                    'Остался последний шаг. Дайте имя вашему кораблю-государству. Оно навсегда войдёт в историю и будет отображаться в галактических реестрах.',
                    'Введите уникальное название для своей колонии'
                ]} sx={{ textAlign: 'center' }} />
                <YagoCardSContentInputField name={name} handleChange={handleNameChange} error={nameError} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <Button variant="contained" onClick={handleSave} disabled={isSending || !name} >
                    {isSending ? <CircularProgress size={24} /> : 'Сохранить'}
                </Button>
            </YagoCard>
        )
    }

    const renderCard = () => {
        switch (step) {
            case 0:
                return renderLoreCard();
            case 1:
                return renderShipCard();
            case 2:
                return renderHangarCard();
            case 3:
                return renderPresetsCard();
            case 4:
                return renderNameCard();
            default:
                return renderLoreCard();
        }
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

export default CreateClolonyPage;