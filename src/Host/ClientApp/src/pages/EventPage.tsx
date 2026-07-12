import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { X, HelpCircle, Clock, ArrowLeft } from 'lucide-react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { QuestType, useCompleteQuestMutation, useGetColonyQuestQuery } from '../entities/MyQuest';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import type { Slide, SlideButton, SlideButtonAction } from '../entities/Episode';
import type { ColonyParameter } from '../entities/ColonyParameter';
import Text from '../shared/Text';
import Button from '../shared/Button';
import ColonyParameterRowList from '../features/ColonyParameterList';
import InputText from '../shared/InputText';
import { formatTimeAgo } from '../features/TimeHelper';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/FlexContainer';

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
    const [slideHistory, setSlideHistory] = useState<string[]>([]);
    const scrollContainerRef = React.useRef<HTMLDivElement>(null);

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
        setSlideHistory([]);
        setInputTextValue('');
        setInputTextError('');
    }, [id]);

    useEffect(() => {
        if (scrollContainerRef.current) {
            scrollContainerRef.current.scrollTop = 0;
        }
    }, [slideIndex]);

    const slides = episode?.slides;
    const currentSlide = slides == undefined ? undefined : slides[slideIndex] || slides[0];
    const hasTextInput = currentSlide?.textInput != undefined;

    // ============================================
    // Логика
    // ============================================
    const handleSetSlideId = (slideId: string) => {
        if (slides == undefined) return;
        const index = slides.findIndex(x => x.id === slideId);
        if (index !== -1) {
            setSlideHistory(prev => [...prev, currentSlide?.id || '']);
            setSlideIndex(index);
        }
    };

    const handleGoBack = () => {
        if (slides == undefined || slideHistory.length === 0)
            return;
        const prevSlideId = slideHistory.pop();
        const index = slides.findIndex(x => x.id === prevSlideId);
        if (index !== -1) {
            setSlideIndex(index);
            setSlideHistory([...slideHistory]);
        }
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

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;
        return (
            <div className="w-full">
                <ColonyParameterRowList items={parameters} dense={true} />
            </div>
        );
    };

    const renderButtons = () => {
        const buttons = currentSlide?.buttons || [];

        return (
            <div className="flex flex-col gap-2 w-full">
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
                                size="sm"
                                sizeMd="md"
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
                                    className="flex-shrink-0 w-10 h-10 rounded-lg border border-bright/20 
                                        text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
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
        return (
            <div
                className={
                    `flex flex-col transition-all duration-300 flex-shrink-0 mt-2 overflow-hidden`}
            >
                {/* Фиксированная часть */}
                <div className="flex-shrink-0 py-2 space-y-2 mx-auto w-full">
                    {hasTextInput && (
                        <InputText
                            name="questInputText"
                            label="Название колонии"
                            type="text"
                            value={inputTextValue}
                            handleChange={handleInputTextChange}
                            handleBlur={handleInputTextChange}
                            error={!!inputTextError}
                            helperText={inputTextError}
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

    const renderCentralPart = () => {
        return (
            <div className={`
                min-h-full w-full max-w-5xl mx-auto
                bg-dark/40 backdrop-blur-sm border border-bright/5
            `}
            >
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/${currentSlide?.imageName}.jpg`}
                        alt={episode?.slides[0]?.title || 'Иллюстрация'}
                        className="w-full h-auto object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>

                <div className='px-4'>
                    <div className="space-y-2 w-full">
                        {currentSlide?.text.map((item, index) => (
                            <Text key={index} size="sm" align='left' className="leading-relaxed">
                                {item}
                            </Text>
                        ))}
                    </div>
                    {renderParameters(currentSlide?.parameters ?? [])}
                </div>
            </div>
        );
    };

    const renderContent = () => (
        <FlexContainer className='h-full max-w-5xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={episode?.slides[0]?.title || 'Событие'}
                    leftButton={{ icon: ArrowLeft, onClick: () => handleGoBack(), label: 'Назад', disabled: slideHistory.length === 0 }}
                    rightButton={{ icon: X, onClick: () => navigate(-1), label: 'Закрыть' }}
                />
            </div>

            <div
                ref={scrollContainerRef}
                className="flex-1 w-full overflow-y-auto scrollbar-hide z-10 relative"
            >
                {renderCentralPart()}
            </div>

            <div className="w-full sticky bottom-0 flex-shrink-0 z-20 border-t border-bright/10">
                {renderBottomPanel()}
            </div>
        </FlexContainer>
    );

    return (
        <Page backgroundImage="space" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default EventPage;