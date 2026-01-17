import React, { useState } from 'react';
import { Button, CircularProgress } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import YagoCard from '../shared/YagoCard';
import YagoButton from '../shared/YagoButton';
import TextMain from '../shared/TextMain';
import StateList from '../shared/StateList';
import { StateItemPopulation, StateItemShip, StateItemSolar, StateItemZones } from '../entities/StateItem';
import YagoCardContentInputField from '../shared/YagoCardContentInputField';
import { ValidateColonyName, SanitizeColonyName } from '../features/ColonyNameValidator';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import SlideCard from '../features/SlideCard';
import { ColonyPresetType, useCreateColonyMutation } from '../entities/ColonyActions';

interface PresetOption {
    presetType: ColonyPresetType;
    label: string;
    image: string;
    description: string;
    comment: string;
    income: number,
    population: number
}

const CreateClolonyPage: React.FC = () => {
    const navigate = useNavigate();

    const error: FetchBaseQueryError | SerializedError | undefined = undefined;

    const [showPresetsSlide, setShowPresetsSlide] = useState<boolean>(false);

    const [createColony, { isLoading }] = useCreateColonyMutation();
    const [step, setStep] = useState<number>(0);
    const [name, setName] = useState('');
    const [nameError, setNameError] = useState('');

    const [colonyPresetType, setColonyPresetType] = useState<ColonyPresetType>(ColonyPresetType.Humanist);

    const presets: PresetOption[] = [
        {
            presetType: ColonyPresetType.Humanist,
            label: 'Инженерная Команда',
            image: 'buildings/1',
            description: 'Ваша стратегия — качество, а не количество. Вы закупаете новейшие буровые дроны у AUTOMATIC SYSTEMS и нанимаете немногочисленных, но блестящих инженеров-операторов, предлагая им контракты с условиями для переезда семей. Этот путь требует крупных начальных вложений и высоких текущих затрат, но закладывает основу для «Привилегированного» статуса YAGO и максимальной эффективности добычи в будущем. Вы строите не просто рудник, а демонстрацию технологического превосходства.',
            comment: 'Передовое оборудование AS и горстка высокооплачиваемых специалистов. Дорого, престижно, эффективно.',
            income: +960,
            population: 320,
        },
        {
            presetType: ColonyPresetType.Pragmatist,
            label: 'Горнодобывающая Бригада',
            image: 'buildings/2',
            description: 'Вы следуете устоявшемуся плану: закупаете проверенные виброкирки и бульдозер, а также нанимаете целую бригаду рудокопов через агентства с лицензией ОПЗ. Это не прорыв, а уверенный шаг. Такой подход сигнализирует регуляторам о вашей благонадёжности, что является самым быстрым путём к стабильному «Стандартному» рейтингу. Вы выбираете предсказуемость и раннюю окупаемость.',
            comment: 'Надёжное оборудование, бригада лицензированных рудокопов ОПЗ. Сбалансированный и предсказуемый старт.',
            income: +980,
            population: 400,
        },
        {
            presetType: ColonyPresetType.Dictator,
            label: 'Реабилитационный Контингент',
            image: 'buildings/3',
            description: 'Ваш расчёт строится на предельной экономии. Вы приобретаете самое простое оборудование, а в качестве рабочей силы используете контингент по программе трудовой реабилитации ОПЗ — должников и заключённых. Понимая риски, вы одновременно нанимаете отряд надзирателей для поддержания порядка. Этот путь позволяет начать с минимальным капиталом, но ваш рейтинг YAGO, вероятно, надолго останется «под наблюдением», а управление колонией будет сведено к подавлению недовольства и контролю за дисциплиной.',
            comment: 'Дешёвое оборудование, контингент должников ОПЗ и обязательный надзор. Дёшево, рискованно, требует жёсткого контроля.',
            income: +1000,
            population: 480,
        }
    ];

    const getCurrentPresetIndex = () =>
        presets.findIndex(r => r.presetType === colonyPresetType);

    const handleNextPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const nextIndex = (currentIndex + 1) % presets.length;
        setColonyPresetType(presets[nextIndex].presetType);
    };

    const handlePrevPreset = () => {
        const currentIndex = getCurrentPresetIndex();
        const prevIndex = (currentIndex - 1 + presets.length) % presets.length;
        setColonyPresetType(presets[prevIndex].presetType);
    };

    const handleSaveColony = async () => {
        try {
            await createColony({ name: name, presetType: colonyPresetType }).unwrap();
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
                    'Теперь и вы обладатель собственного корабля. Серийный, неказистый, но полностью функциональный корабль-город с добывающим комплексом.',
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
                <StateList items={[StateItemZones('Сектора', `0 / 140`)]} sx={{ mb: '8px' }} />
                <TextMain textArray={[
                    '14 000 квадратных метров пустого пространства. Здесь будут жить те, чьим трудом выстроится ваше богатство.',
                    'Вам предстоит решить: в каких условиях они будут существовать, какие законы будут ими управлять и какое общество вы создадите на этом клочке стали, затерянном в пустоте космоса.'
                ]} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <YagoButton onClick={() => setStep(step + 1)} text={'Далее'} isDisabled={false} />
            </YagoCard>
        )
    }

    const renderPresetsCard = () => {
        const currentPreset = presets.find(r => r.presetType === colonyPresetType)!;
        const image = currentPreset.image;

        if (showPresetsSlide)
            return <SlideCard
                slide={{
                    id: currentPreset.presetType,
                    title: currentPreset.label,
                    imageName: currentPreset.image,
                    text: [currentPreset.description],
                    footer: currentPreset.comment
                }}
                closeAction={() => setShowPresetsSlide(false)}
            />

        return (
            <YagoCard
                title='Первые колонисты'
                image={`/assets/images/${image ?? 'home'}.jpg`}
            >
                <TextMain textArray={['Наймите первых работников-колонистов']} sx={{ textAlign: 'center' }} />
                <YagoCardContentSelection handlePrev={handlePrevPreset} label={currentPreset.label} handleNext={handleNextPreset} />
                <TextMain textArray={[currentPreset.comment]} sx={{ textAlign: 'justify' }} />
                <StateList
                    items={[
                        StateItemSolar('Солары', `1 000 (${currentPreset.income} /ц)`),
                        StateItemZones('Сектора', `40 / 140`),
                        StateItemPopulation('Население', `${currentPreset.population} чел.`),
                    ]}
                    sx={{ mb: '8px' }} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <YagoButton variant="contained" onClick={() => setStep(step + 1)} text={'Выбрать'} />
                <YagoButton onClick={() => setShowPresetsSlide(true)} text={'Описание'} />
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
                    'Остался последний шаг. Дайте имя вашей колонии. Оно навсегда войдёт в историю и будет отображаться в галактических реестрах.',
                    'Можно использовать: латинские буквы, цифры, пробелы, дефисы, апострофы и точки. Длина: от 3 до 16 символов.'
                ]} />
                <YagoCardContentInputField name={name} handleChange={handleNameChange} error={nameError} />
                <YagoButton onClick={() => setStep(step - 1)} text={'Назад'} isDisabled={false} />
                <Button variant="contained" onClick={handleSave} disabled={isLoading || !name} >
                    {isLoading ? <CircularProgress size={24} /> : 'Сохранить'}
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