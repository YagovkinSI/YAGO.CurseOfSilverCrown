import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { X, HelpCircle, Clock } from 'lucide-react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { QuestType, useCompleteQuestMutation, useGetColonyQuestQuery } from '../entities/MyQuest';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import type { Slide, SlideButton, SlideButtonAction } from '../entities/Episode';
import type { ColonyParameter } from '../entities/ColonyParameter';
import PageContainer from '../widgets/ContainerPage';
import Text from '../shared/Text';
import Button from '../shared/Button';
import ButtonBack from '../shared/ButtonBack';
import ColonyParameterRowList from '../features/ColonyParameterList';
import InputText from '../shared/InputText';
import { formatTimeAgo } from '../features/TimeHelper';

const EventPage: React.FC = () => {
    const { id } = useParams();
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const navigate = useNavigate();
    const myUserDataResult = useGetMyUserQuery();
    const colonyQuestResult = useGetColonyQuestQuery(id ?? "");
    const [completeQuestMutation, completeQuestResult] = useCompleteQuestMutation();
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);
    const [isExpanded, setIsExpanded] = useState(false);

    const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading || completeQuestResult.isLoading;
    const error = myUserDataResult.error ?? colonyQuestResult.error ?? completeQuestResult.error ?? handleChoiceError;
    const episode = completeQuestResult.data?.data ?? colonyQuestResult.data?.data?.episode;
    const canBeClosed = completeQuestResult.data != undefined || colonyQuestResult.data?.data?.type !== QuestType.Immediately;
    const questCreatedAt = colonyQuestResult.data?.data?.createdAt;

    useEffect(() => {
        if (!myUserDataResult.isLoading && !myUserDataResult.data?.data) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    useEffect(() => {
        setSlideIndex(0);
        setInputTextValue('');
        setInputTextError('');
    }, [id]);

    const slides = episode?.slides;
    const currentSlide = slides == undefined ? undefined : slides[slideIndex] || slides[0];
    const hasTextInput = currentSlide?.textInput != undefined;

    // ============================================
    // Логика
    // ============================================
    const handleSetSlideId = (slideId: string) => {
        if (slides == undefined) return;
        const index = slides.findIndex(x => x.id === slideId);
        if (index !== -1) setSlideIndex(index);
    };

    const handleSetChoice = async (action: SlideButtonAction, inputTextValue?: string) => {
        try {
            const result = await completeQuestMutation({
                id: action.arguments[0],
                dilemmaResolving: inputTextValue ?? action.arguments[1]
            }).unwrap();
            if (result.data == undefined) {
                navigate('/me');
            }
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setHandleChoiceError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setHandleChoiceError('Неизвестная ошибка.');
            }
        }
    };

    const handleInputTextSave = async (action: SlideButtonAction) => {
        const sanitizedValue = SanitizeColonyName(inputTextValue);
        setInputTextValue(sanitizedValue);
        const validationResult = ValidateColonyName(sanitizedValue);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        } else {
            setInputTextError('');
            await handleSetChoice(action, sanitizedValue);
        }
    };

    const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setInputTextValue(value);
        if (value.length > 2) {
            const validationResult = ValidateColonyName(value);
            setInputTextError(validationResult.isValid ? '' : validationResult.error!);
        } else {
            setInputTextError('');
        }
    };

    // ============================================
    // Рендеры
    // ============================================
    const renderHeader = () => (
        <div className="flex items-center justify-between w-full mb-4">
        {/* <div className="flex items-center justify-between px-4 py-3 bg-transparent flex-shrink-0"> */}
            <ButtonBack />
            <h1 className="text-lg font-bold text-light">
                {episode?.slides[0]?.title || 'Событие'}
            </h1>
            {/* <Title className="text-center truncate max-w-[60%]"></Title> */}
            <button
                onClick={() => navigate(-1)}
                className="p-2 text-muted hover:text-light transition-colors"
                aria-label="Закрыть"
            >
                <X className="w-5 h-5" />
            </button>
        </div>
    );

    const renderSlideIndicator = () => {
        if (slides == undefined || slides.length <= 1) return null;

        return (
            <div className="flex justify-center gap-1.5 px-4 py-2 flex-shrink-0">
                {slides.map((_: any, idx: number) => (
                    <div
                        key={idx}
                        className={`h-1 rounded-full transition-all duration-300 ${idx === slideIndex ? 'w-6 bg-bright' : 'w-1.5 bg-bright/20'
                            }`}
                    />
                ))}
            </div>
        );
    };

    const renderImage = () => {
        if (!currentSlide?.imageName) return null;
        return (
            <div className="relative w-full flex-shrink-0 overflow-hidden rounded-xl max-h-[40vh]">
                <img
                    src={`/images/pictures/${currentSlide.imageName}.jpg`}
                    alt={episode?.slides[0]?.title || 'Событие'}
                    className="w-full h-full object-cover object-center"
                    loading="lazy"
                />
                <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
            </div>
        );
    };

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;
        return (
            <div className="w-full max-w-md mx-auto">
                <ColonyParameterRowList items={parameters} dense={true} />
            </div>
        );
    };

    const renderButtons = () => {
        const buttons = currentSlide?.buttons || [];

        return (
            <div className="flex flex-col gap-2 w-full max-w-md mx-auto">
                {buttons.map((button: SlideButton, index: number) => {
                    const isMutation = button.action != undefined;

                    const handleClick = () => {
                        if (button.action) {
                            if (hasTextInput) {
                                handleInputTextSave(button.action);
                            } else {
                                handleSetChoice(button.action);
                            }
                        } else if (button.navigate) {
                            navigate(button.navigate.actionUrl);
                        } else if (button.toSlide) {
                            handleSetSlideId(button.toSlide.slideId);
                        }
                    };

                    return (
                        <div key={index} className="flex items-center gap-2">
                            <Button
                                variant={isMutation ? 'primary' : 'secondary'}
                                size="md"
                                onClick={handleClick}
                                disabled={!button.isAvailable}
                                className="flex-1"
                            >
                                {button.name}
                            </Button>
                            {button.infoSlideId && (
                                <button
                                    onClick={() => {
                                        if (slides == undefined) return;
                                        const idx = slides.findIndex((s: Slide) => s.id === button.infoSlideId);
                                        if (idx !== -1) setSlideIndex(idx);
                                    }}
                                    className="flex-shrink-0 w-10 h-10 rounded-lg border border-bright/20 text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                                    aria-label="Подробнее"
                                >
                                    <HelpCircle className="w-4 h-4" />
                                </button>
                            )}
                        </div>
                    );
                })}
            </div>
        );
    };

    const renderBottomPanel = () => {
        const panelHeight = isExpanded ? '70vh' : '40vh';

        return (
            <div
                className="bg-dark/90 backdrop-blur-sm border border-bright/10 rounded-2xl flex flex-col transition-all duration-300 flex-shrink-0 mt-2 overflow-hidden"
                style={{ maxHeight: panelHeight }}
            >
                {/* Индикатор + кнопка разворачивания */}
                <div className="flex items-center justify-between px-4 py-1 flex-shrink-0">
                    {renderSlideIndicator()}
                    <button
                        onClick={() => setIsExpanded(!isExpanded)}
                        className="text-muted hover:text-light transition-colors p-1"
                        aria-label={isExpanded ? 'Свернуть' : 'Развернуть'}
                    >
                        {isExpanded ? '▼' : '▲'}
                    </button>
                </div>

                {/* Скроллируемая часть */}
                <div className="flex-1 min-h-0 overflow-y-auto px-4 py-3 space-y-4 max-w-2xl mx-auto w-full">
                    <Text size="sm" className="leading-relaxed md:leading-normal">
                        {currentSlide?.text}
                    </Text>
                    {renderParameters(currentSlide?.parameters ?? [])}
                </div>

                {/* Фиксированная часть */}
                <div className="flex-shrink-0 px-4 py-3 space-y-3 max-w-2xl mx-auto w-full border-t border-bright/10">
                    {hasTextInput && (
                        <InputText
                            name="questInputText"
                            label="Название колонии"
                            type="text"
                            value={inputTextValue}
                            handleChange={handleInputTextChange}
                            handleBlur={handleInputTextChange}
                            error={!!inputTextError}
                            helperText={inputTextError || 'Название колонии'}
                        />
                    )}

                    {renderButtons()}

                    {canBeClosed && (
                        <Button variant="secondary" size="sm" onClick={() => navigate(-1)}>
                            Закрыть
                        </Button>
                    )}

                    {questCreatedAt && (
                        <div className="flex items-center gap-2 pt-1">
                            <Clock className="w-3 h-3 text-muted/30" />
                            <span className="text-[0.55rem] text-muted/30">
                                {formatTimeAgo(questCreatedAt)}
                            </span>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    const renderContent = () => (
        <div className="w-full h-full max-w-2xl mx-auto px-4 py-4">
        {/* </div><div className="flex flex-col h-full px-4"> */}
            {renderHeader()}
            {currentSlide?.imageName && renderImage()}
            {renderBottomPanel()}
        </div>
    )

    return (
        <PageContainer backgroundImage="space" darkenBackground isLoading={isLoading} error={error} justifyContent='start'>
            {renderContent()}
        </PageContainer>
    );
};

export default EventPage;