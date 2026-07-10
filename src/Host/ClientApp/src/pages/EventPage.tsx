import SlideCard from '../shared/SlideCard';
import { useEffect, useState } from 'react';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import Button from '../shared/Button';
import { QuestType, useCompleteQuestMutation, useGetColonyQuestQuery } from '../entities/MyQuest';
import Text from '../shared/Text';
import type { ColonyParameter } from '../entities/ColonyParameter';
import ColonyParameterRowList from '../features/ColonyParameterList';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import type { SlideButton, SlideButtonAction } from '../entities/Episode';
import PageContainer from '../widgets/ContainerPage';
import InputText from '../shared/InputText';

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

    const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading;
    const error = myUserDataResult.error ?? colonyQuestResult.error ?? handleChoiceError;
    const episode = completeQuestResult.data?.data ?? colonyQuestResult.data?.data?.episode;
    const canBeClosed = completeQuestResult.data != undefined || colonyQuestResult.data?.data?.type != QuestType.Immediately;

    useEffect(() => {
        if (!(myUserDataResult.data?.data != undefined)) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    const handleSetSlideId = (slideId: string) => {
        const index = episode?.slides?.findIndex(x => x.id == slideId);
        if (index == undefined || index == -1) return;
        setSlideIndex(index);
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
            if (e && typeof e == 'object' && 'data' in e) {
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

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (parameters.length == 0) return null;
        return (
            <div className="flex flex-col gap-1 w-full max-w-[350px] md:max-w-[700px] mx-auto">
                <ColonyParameterRowList items={parameters} />
            </div>
        );
    };

    const renderSlideButton = (button: SlideButton, withTextInput: boolean) => {
        const isMutation = button.action != undefined;

        const handleClick = () => {
            if (button.action != undefined) {
                if (withTextInput) {
                    handleInputTextSave(button.action);
                } else {
                    handleSetChoice(button.action);
                }
            } else if (button.navigate != undefined) {
                navigate(button.navigate.actionUrl);
            } else if (button.toSlide != undefined) {
                handleSetSlideId(button.toSlide.slideId);
            }
        };

        return (
            <Button
                variant={isMutation ? 'primary' : 'secondary'}
                onClick={handleClick}
                disabled={!button.isAvailable}
            >
                {button.name}
            </Button>
        );
    };

    const renderButtons = (buttons: SlideButton[], withTextInput: boolean) => (
        <div className="flex flex-col gap-3 items-center w-full">
            {buttons.map((button, index) => (
                <React.Fragment key={index}>
                    {renderSlideButton(button, withTextInput)}
                </React.Fragment>
            ))}
        </div>
    );

    const renderContent = () => {
        if (episode?.slides == undefined || episode.slides.length == 0)
            return;
        const slide = episode.slides[slideIndex];
        const hasTextInput = slide.textInput != undefined;

        return (
            <SlideCard
                title={slide.title}
                image={`/images/pictures//${slide.imageName}.jpg`}
            >
                <div className="flex flex-col gap-4 items-center">
                    <Text>
                        {slide.text}
                    </Text>
                    {renderParameters(slide.parameters)}
                    {hasTextInput && (
                        <InputText
                            name="questInputText"
                            label="Название колонии"
                            type="text"
                            value={inputTextValue}
                            handleChange={handleInputTextChange}
                            handleBlur={handleInputTextChange}
                            error={inputTextError != ''}
                            helperText={inputTextError ?? 'Название колонии'}
                        />
                    )}
                    {renderButtons(slide.buttons, hasTextInput)}
                    {canBeClosed && (
                        <Button onClick={() => navigate(-1)} variant="secondary">
                            Закрыть
                        </Button>
                    )}
                </div>
            </SlideCard>
        );
    };

    return (
        <PageContainer backgroundImage='space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default EventPage;