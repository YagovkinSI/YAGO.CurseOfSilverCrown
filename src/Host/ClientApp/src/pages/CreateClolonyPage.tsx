import React, { useState } from 'react';
import { CircularProgress } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import YagoCard from '../shared/YagoCard';
import YagoButton from '../shared/YagoButton';
import TextMain from '../shared/TextMain';
import YagoCardContentInputField from '../shared/YagoCardContentInputField';
import { ValidateColonyName, SanitizeColonyName } from '../features/ColonyNameValidator';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import SlideCard from '../features/SlideCard';
import { ColonyPresetType } from '../entities/ColonyParameter';

interface PresetOption {
    presetType: ColonyPresetType;
    label: string;
    image: string;
    description: string[];
    comment: string;
    income: string;
    codeOfLaws: string
}

const CreateClolonyPage: React.FC = () => {
    const navigate = useNavigate();

    const error: FetchBaseQueryError | SerializedError | undefined = undefined;

    const [showPresetsSlide, setShowPresetsSlide] = useState<boolean>(false);

    const [step, setStep] = useState<number>(0);
    const [name, setName] = useState('');
    const [nameError, setNameError] = useState('');

    const [colonyPresetType, setColonyPresetType] = useState<ColonyPresetType>(ColonyPresetType.Centrist);

    const isLoading = false;

    const presets: PresetOption[] = [
        {
            presetType: ColonyPresetType.Humanist,
            label: 'Гуманистический Устав',
            image: 'gavernorType/1',
            description: [
                'Этот свод, разработанный прогрессивным крылом ОПЗ, жёстко регламентирует качество жизни: объём жилплощади, нормы питания, медицинское обеспечение и безопасность труда. Чтобы компенсировать расходы резидентов на эти стандарты, базовые налоги для бизнеса установлены на минимальном уровне. Колония, основанная на Уставе, становится магнитом для лучших специалистов и образцом для ОПЗ, быстро продвигаясь к «Привилегированному» статусу YAGO. Однако высокие операционные издержки делают её непривлекательной для дешёвой рабочей силы и рискованных проектов.'
            ],
            comment: 'Приоритет — благополучие колонистов. Высокие стандарты жизни, низкие налоги, путь к престижу.',
            income: 'Низкие',
            codeOfLaws: 'Высокие',
        },
        {
            presetType: ColonyPresetType.Centrist,
            label: 'Стандартный Протокол',
            image: 'gavernorType/2',
            description: [
                'Протокол — это компромиссный каркас, на котором построены тысячи успешных колоний. Он устанавливает чёткие, но выполнимые требования по условиям труда, безопасности и экологии, обеспечивая приемлемый уровень жизни без излишней нагрузки на бизнес. Налоговая ставка сбалансирована. Этот выбор гарантирует, что все основные резидент-компании будут готовы работать с вами, а ОПЗ сочтёт колонию благонадёжной. Это путь к устойчивому развитию без резких взлётов и падений.'
            ],
            comment: 'Универсальный шаблон ОПЗ. Умеренные правила, стабильный рост, предсказуемость.',
            income: 'Средние',
            codeOfLaws: 'Средние',
        },
        {
            presetType: ColonyPresetType.Capitalist,
            label: 'Корпоративный Регламент',
            image: 'gavernorType/3',
            description: [
                'Регламент создан не для людей, а для баланса в отчётах. Он формально соблюдает абсолютный минимум требований ОПЗ, сводя социальные гарантии к нулю, зато предлагает бизнесу «сделку»: вы платите повышенные налоги и сборы, а взамен получаете практически полную свободу действий внутри своих секторов и минимальное вмешательство инспекций. Это привлекает авантюристов, контрактные агентства и теневиков, для которых важна невысокая цена вопроса и отсутствие лишних глаз. Такая колония быстро наполняет казну, но становится социальной пороховой бочкой и потенциальным клиентом для услуг «Чёрной Марки», видящей в вас родственную душу.'
            ],
            comment: 'Максимизация дохода. Высокие налоги, слабые регуляции, высокие риски.',
            income: 'Высокие',
            codeOfLaws: 'Низкие',
        }
    ];

    const getCurrentPresetIndex = () =>
        presets.findIndex(r => r.presetType === colonyPresetType);

    const handleNextPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const nextIndex = (currentIndex - 1 + presets.length) % presets.length;
        setColonyPresetType(presets[nextIndex].presetType);
    };

    const handlePrevPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const prevIndex = (currentIndex + 1) % presets.length;
        setColonyPresetType(presets[prevIndex].presetType);
    };

    const handleSaveColony = async () => {
        try {
            //await createColony({ name: name, presetType: colonyPresetType }).unwrap();
            navigate('/me/colony');
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setNameError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setNameError('Неизвестная ошибка.');
            }
        }
    };

    const validateName = (value: string): boolean => {
        const validationResult = ValidateColonyName(value);
        if (!validationResult.isValid) {
            setNameError(validationResult.error!);
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
            const sanitazed = SanitizeColonyName(value);
            setName(sanitazed);
        } else {
            setNameError('');
        }
    };

    const handleSave = () => {
        if (validateName(name) && name == SanitizeColonyName(name)) {
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
                <YagoButton onClick={() => setStep(step + 1)}>Далее</YagoButton>
            </YagoCard>
        )
    }

    const renderShipCard = () => {
        return (
            <YagoCard
                title='Ваш Актив'
                image={`/assets/images/pictures/ship_1.jpg`}
            >
                <TextMain textArray={[
                    'Теперь и вы обладатель собственного корабля. Серийный, неказистый, но полностью функциональный корабль-город с добывающим комплексом.',
                    'Его цеха готовы к переработке льда и руды в Поясе Астероидов. Но вам нужны люди. Вам нужна колония.'
                ]} />
                <YagoButton onClick={() => setStep(step - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={() => setStep(step + 1)}>Далее</YagoButton>
            </YagoCard>
        )
    }

    const renderHangarCard = () => {
        return (
            <YagoCard
                title='Чистый Лист'
                image={`/assets/images/pictures/empty_hangar.jpg`}
            >
                <TextMain textArray={[
                    '14 000 квадратных метров жилых модулей. Здесь будут жить те, чьим трудом выстроится ваше богатство.',
                    'Вам предстоит решить: в каких условиях они будут существовать, какие законы будут ими управлять и какое общество вы создадите на этом клочке стали, затерянном в пустоте космоса.'
                ]} />
                <YagoButton onClick={() => setStep(step - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={() => setStep(step + 1)}>Далее</YagoButton>
            </YagoCard>
        )
    }

    const renderPresetsCard = () => {
        const currentPreset = presets.find(r => r.presetType === colonyPresetType)!;
        const image = currentPreset.image;

        if (showPresetsSlide)
            return <SlideCard
                slide={{
                    title: currentPreset.label,
                    imageName: `pictures/${currentPreset.image}`,
                    text: currentPreset.description,
                    parameters: [],
                    buttonName: "Выбрать",
                    footer: currentPreset.comment
                }}
                closeAction={() => setShowPresetsSlide(false)}
            />

        return (
            <YagoCard
                title='Свод законов'
                image={`/assets/images/pictures/${image ?? 'home'}.jpg`}
            >
                <TextMain textArray={['Заложите Фундамент Законов']} sx={{ textAlign: 'center' }} />
                <YagoCardContentSelection handlePrev={handlePrevPreset} label={currentPreset.label} handleNext={handleNextPreset} />
                <TextMain textArray={[currentPreset.comment]} sx={{ textAlign: 'justify' }} />
                <YagoButton onClick={() => setStep(step - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={() => setStep(step + 1)} type='mutation'>Выбрать</YagoButton>
                <YagoButton onClick={() => setShowPresetsSlide(true)} type='secondary'>Описание</YagoButton>
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
                    'Остался последний шаг. Дайте имя вашей колонии. Оно навсегда войдёт в историю и будет отображаться в галактических реестрах.'
                ]} />
                <YagoCardContentInputField value={name} label='Название колонии' handleChange={handleNameChange} error={nameError} />
                <YagoButton onClick={() => setStep(step - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={handleSave} isDisabled={isLoading || !name} type='mutation'  >
                    {isLoading ? <CircularProgress size={24} /> : 'Сохранить'}
                </YagoButton>
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