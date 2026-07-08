import React, { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, X, AlertCircle, Clock, ChevronDown, ChevronUp } from 'lucide-react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { QuestType, useCompleteQuestMutation, useGetColonyQuestQuery } from '../entities/MyQuest';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import { type SlideButton, type SlideButtonAction } from '../entities/Episode';
import PageContainer from '../shared/PageContainer';
import YagoText from '../shared/YagoText';
import YagoButton from '../shared/YagoButton';
import { formatTimeAgo } from '../features/TimeHelper';
import type { ColonyParameter } from '../entities/ColonyParameter';

const EventPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const myUserDataResult = useGetMyUserQuery();
    const colonyQuestResult = useGetColonyQuestQuery(id ?? "");
    const [completeQuestMutation, completeQuestResult] = useCompleteQuestMutation();

    const [slideIndex, setSlideIndex] = useState<number>(0);
    const [showInfoSlide, setShowInfoSlide] = useState<string | null>(null);
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);
    const [isExpanded, setIsExpanded] = useState(false);
    const [showScrollHint, setShowScrollHint] = useState(false);
    const scrollContainerRef = useRef<HTMLDivElement>(null);

    const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading || completeQuestResult.isLoading;
    const error = myUserDataResult.error ?? colonyQuestResult.error ?? completeQuestResult.error ?? handleChoiceError;

    const episode = completeQuestResult.data?.data ?? colonyQuestResult.data?.data?.episode;
    const canBeClosed = completeQuestResult.data != undefined || colonyQuestResult.data?.data?.type !== QuestType.Immediately;
    const slides = episode?.slides || [];
    const currentSlide = slides.length > 0 ? slides[slideIndex] || slides[0] : undefined;

    // Редирект если не авторизован
    useEffect(() => {
        if (!myUserDataResult.isLoading && !myUserDataResult.data?.data) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    // Сброс индекса при смене эпизода
    useEffect(() => {
        setSlideIndex(0);
        setInputTextValue('');
        setInputTextError('');
    }, [id]);

    useEffect(() => {
        const container = scrollContainerRef.current;
        if (!container) return;

        const checkScroll = () => {
            const { scrollTop, scrollHeight, clientHeight } = container;
            const isScrolled = scrollHeight > clientHeight;
            const isAtBottom = scrollTop + clientHeight >= scrollHeight - 10;
            setShowScrollHint(isScrolled && !isAtBottom);
        };

        checkScroll();
        container.addEventListener('scroll', checkScroll);
        return () => container.removeEventListener('scroll', checkScroll);
    }, [currentSlide]);

    if (!episode) {
        return (
            <PageContainer backgroundImage="space">
                <div className="flex flex-col items-center justify-center h-full">
                    <AlertCircle className="w-12 h-12 text-danger" />
                    <YagoText variant="primary" size="lg" className="mt-4">
                        Событие не найдено
                    </YagoText>
                    <YagoButton variant="secondary" className="mt-4" onClick={() => navigate(-1)}>
                        Назад
                    </YagoButton>
                </div>
            </PageContainer>
        );
    }

    // ============================================
    // Логика (из старого кода)
    // ============================================
    const handleSetSlideId = (slideId: string) => {
        const index = slides?.findIndex(x => x.id === slideId);
        if (index !== undefined && index !== -1) {
            setSlideIndex(index);
        }
    };

    const handleSetChoice = async (action: SlideButtonAction, inputTextValue?: string) => {
        try {
            const result = await completeQuestMutation({
                id: action.arguments[0],
                dilemmaResolving: inputTextValue ?? action.arguments[1]
            }).unwrap();
            if (result.data == undefined) {
                navigate('/me/colony');
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
        const sanitized = SanitizeColonyName(inputTextValue);
        setInputTextValue(sanitized);
        const validationResult = ValidateColonyName(sanitized);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        } else {
            await handleSetChoice(action, sanitized);
        }
    };

    const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setInputTextValue(value);
        if (value.length > 2) {
            validateInputText(value);
        } else {
            setInputTextError('');
        }
    };

    const validateInputText = (value: string): boolean => {
        const validationResult = ValidateColonyName(value);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
            return false;
        }
        setInputTextError('');
        return true;
    };

    // ============================================
    // Рендеры
    // ============================================
    const renderHeader = () => (
        <div className="flex items-center justify-between w-full px-4 py-3 bg-dark/60 backdrop-blur-sm border-b border-bright/10">
            <button
                onClick={() => navigate(-1)}
                className="text-muted hover:text-light transition-colors p-1"
                aria-label="Назад"
            >
                <ArrowLeft className="w-5 h-5" />
            </button>
            <h1 className="text-sm font-medium text-light truncate max-w-[60%]">
                {episode?.slides[0].title || 'Событие'}
            </h1>
            <button
                onClick={() => navigate(-1)}
                className="text-muted hover:text-light transition-colors p-1"
                aria-label="Закрыть"
            >
                <X className="w-5 h-5" />
            </button>
        </div>
    );

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;

        return (
            <div className="bg-dark/50 border border-bright/10 rounded-lg p-3">
                {/*<p className="text-xs text-muted/50 uppercase tracking-wider mb-2">Требования:</p>*/}
                <div className="space-y-1">
                    {parameters.map((req, idx) => (
                        <div key={idx} className="flex items-center gap-2 text-sm">
                            {/*req.isMet ? (
                                <Check className="w-4 h-4 text-good flex-shrink-0" />
                            ) : (
                                <AlertCircle className="w-4 h-4 text-danger flex-shrink-0" />
                            )*/}
                            <span className={/*req.isMet ?*/ 'text-muted' /*: 'text-light'*/}>
                                {req.name}: {req.value}
                            </span>
                        </div>
                    ))}
                </div>
            </div>
        );
    };

    const renderTextInput = () => {
        const textInput = currentSlide?.textInput;
        if (!textInput) return null;

        return (
            <div className="space-y-2">
                <input
                    type="text"
                    value={inputTextValue}
                    onChange={handleInputTextChange}
                    placeholder={textInput.preload || 'Введите название...'}
                    className={`w-full px-4 py-3 bg-dark/50 border rounded-lg text-light placeholder-muted focus:outline-none transition-colors ${inputTextError ? 'border-danger focus:border-danger' : 'border-bright/20 focus:border-bright/50'
                        }`}
                />
                {inputTextError && (
                    <p className="text-xs text-danger">{inputTextError}</p>
                )}
            </div>
        );
    };

    const renderButtons = () => {
        const buttons = currentSlide?.buttons || [];
        const hasTextInput = !!currentSlide?.textInput;

        return (
            <div className="flex flex-col gap-1.5">
                {buttons.map((btn: SlideButton) => {
                    const isMutation = btn.action != undefined;

                    const handleClick = () => {
                        if (btn.action) {
                            if (hasTextInput) {
                                handleInputTextSave(btn.action);
                            } else {
                                handleSetChoice(btn.action);
                            }
                        } else if (btn.navigate) {
                            navigate(btn.navigate.actionUrl);
                        } else if (btn.toSlide) {
                            handleSetSlideId(btn.toSlide.slideId);
                        }
                    };

                    return (
                        <div key={btn.name} className="flex items-center gap-2">
                            <YagoButton
                                variant={isMutation ? 'primary' : 'secondary'}
                                size="md"
                                onClick={handleClick}
                                disabled={!btn.isAvailable}
                                className="flex-1 py-1.5"
                            >
                                {btn.name}
                            </YagoButton>

                            {/*btn.infoSlideId && (
                                <button
                                    onClick={() => setShowInfoSlide(btn.infoSlideId || null)}
                                    className="flex-shrink-0 w-11 h-11 rounded-lg border border-bright/20 text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                                    aria-label="Подробнее"
                                >
                                    <HelpCircle className="w-5 h-5" />
                                </button>
                            )*/}
                        </div>
                    );
                })}
            </div>
        );
    };

    const renderBottomPanel = () => {
        const panelHeight = isExpanded ? '70vh' : '30vh';

        return (
            <div
                className="bg-dark/80 backdrop-blur-sm border-t border-bright/10 rounded-t-2xl flex flex-col transition-all duration-300"
                style={{ maxHeight: panelHeight }}
            >
                {/* Индикатор слайдов + Кнопка разворачивания */}
                <div className="flex items-center justify-between px-4 py-2 flex-shrink-0">
                    <div className="flex gap-1.5">
                        {slides.map((_: any, idx: number) => (
                            <div
                                key={idx}
                                className={`h-1 rounded-full transition-all duration-300 ${idx === slideIndex
                                        ? 'w-6 bg-bright'
                                        : 'w-1.5 bg-bright/20'
                                    }`}
                            />
                        ))}
                    </div>
                    <button
                        onClick={() => setIsExpanded(!isExpanded)}
                        className="text-muted hover:text-light transition-colors p-1"
                        aria-label={isExpanded ? 'Свернуть' : 'Развернуть'}
                    >
                        {isExpanded ? (
                            <ChevronDown className="w-5 h-5" />
                        ) : (
                            <ChevronUp className="w-5 h-5" />
                        )}
                    </button>
                </div>

                {/* СКРОЛЛИРУЕМАЯ ЧАСТЬ (с индикатором) */}
                <div className="relative flex-1 overflow-hidden">
                    <div
                        ref={scrollContainerRef}
                        className="h-full overflow-y-auto px-4 py-2 space-y-3 max-w-2xl mx-auto w-full scrollbar-thin scrollbar-thumb-bright/30 scrollbar-track-transparent"
                    >
                        {/* Текст */}
                        <div className="space-y-2">
                            {currentSlide?.text?.map((paragraph: string, idx: number) => (
                                <YagoText key={idx} variant="secondary" size="sm" align="left" className="leading-relaxed">
                                    {paragraph}
                                </YagoText>
                            ))}
                        </div>

                        {/* Параметры */}
                        {renderParameters(currentSlide!.parameters)}
                    </div>

                    {/* Градиент-индикатор (если есть скролл) */}
                    {showScrollHint && (
                        <div className="absolute bottom-0 left-0 right-0 h-8 bg-gradient-to-t from-dark/80 to-transparent pointer-events-none" />
                    )}
                </div>

                {/* ФИКСИРОВАННАЯ ЧАСТЬ (кнопки — выше на 1/3) */}
                <div className="flex-shrink-0 px-4 py-3 space-y-2 max-w-2xl mx-auto w-full border-t border-bright/10">
                    {/* Поле ввода текста */}
                    {renderTextInput()}

                    {/* Кнопки — мобильная версия выше */}
                    <div className="space-y-1.5">
                        {renderButtons()}
                    </div>

                    {/* Закрыть */}
                    {canBeClosed && (
                        <YagoButton
                            variant="secondary"
                            size="sm"
                            onClick={() => navigate(-1)}
                            className="mt-1"
                        >
                            Закрыть
                        </YagoButton>
                    )}

                    {/* Подпись */}
                    {colonyQuestResult.data?.data?.createdAt && (
                        <div className="flex items-center gap-2 pt-1">
                            <Clock className="w-3 h-3 text-muted/30" />
                            <span className="text-[0.55rem] text-muted/30">
                                {formatTimeAgo(colonyQuestResult.data.data.createdAt)}
                            </span>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    // Модалка с пояснением
    const renderInfoModal = () => {
        if (!showInfoSlide) return null;
        const info = slides.find((s: any) => s.id === showInfoSlide);
        if (!info) return null;

        return (
            <div className="fixed inset-0 z-[2000] flex items-center justify-center bg-dark/80 backdrop-blur-sm p-4">
                <div className="bg-dark/95 border border-bright/20 rounded-2xl max-w-md w-full p-6 max-h-[80vh] overflow-y-auto">
                    <div className="flex justify-between items-center mb-4">
                        <h3 className="text-lg font-bold text-light">Подробнее</h3>
                        <button
                            onClick={() => setShowInfoSlide(null)}
                            className="text-muted hover:text-light transition-colors"
                        >
                            <X className="w-5 h-5" />
                        </button>
                    </div>
                    <div className="space-y-3">
                        {info.text?.map((paragraph: string, idx: number) => (
                            <p key={idx} className="text-sm text-muted leading-relaxed">
                                {paragraph}
                            </p>
                        ))}
                    </div>
                    <YagoButton variant="secondary" className="mt-4" onClick={() => setShowInfoSlide(null)}>
                        Закрыть
                    </YagoButton>
                </div>
            </div>
        );
    };

    // Ошибка/Загрузка
    if (error && !isLoading) {
        return (
            <PageContainer backgroundImage="space">
                <div className="flex flex-col items-center justify-center h-full px-4">
                    <AlertCircle className="w-12 h-12 text-danger" />
                    <YagoText variant="primary" size="lg" className="mt-4">
                        Ошибка
                    </YagoText>
                    <YagoText variant="secondary" size="sm" className="mt-2 text-center">
                        {String(error)}
                    </YagoText>
                    <YagoButton variant="secondary" className="mt-4" onClick={() => navigate(-1)}>
                        Назад
                    </YagoButton>
                </div>
            </PageContainer>
        );
    }

    if (isLoading || !episode) {
        return (
            <PageContainer backgroundImage="space">
                <div className="flex flex-col items-center justify-center h-full">
                    <div className="w-12 h-12 border-4 border-bright/20 border-t-bright rounded-full animate-spin" />
                    <YagoText variant="secondary" size="sm" className="mt-4">
                        Загрузка...
                    </YagoText>
                </div>
            </PageContainer>
        );
    }

    return (
        <PageContainer backgroundImage={currentSlide?.imageName || 'events'}
        >
            {/* Верхняя панель */}
            {renderHeader()}

            {/* Центр — пустое пространство для фона */}
            <div className="flex-1" />

            {/* Нижняя панель */}
            {renderBottomPanel()}

            {/* Модалка с пояснением */}
            {renderInfoModal()}
        </PageContainer>
    );
};

export default EventPage;