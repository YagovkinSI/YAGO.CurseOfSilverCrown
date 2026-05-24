import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import TextMain from '../shared/TextMain';
import { useRunCycleMutation } from '../entities/MyCycle';
import { useEpisodeActionMutation, type Episode, type Slide, type SlideButton, type SlideButtonAction } from "../entities/Episode";
import YagoCardContentInputField from '../shared/YagoCardContentInputField';
import type { ColonyParameter } from '../entities/ColonyParameter';
import { SanitizeColonyName as SanitizeInpitText, ValidateColonyName as ValidateInpitText } from '../features/ColonyNameValidator';
import { useGetMyColonyQuery } from '../entities/MyColony';
import ColonyParameterList from '../features/ColonyParameterList';

const RunCyclePage: React.FC = () => {
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const myColonyResult = useGetMyColonyQuery();
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const [episodeActionMutation, episodeActionResult] = useEpisodeActionMutation();
    const navigate = useNavigate();
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');

    const isLoading = runCycleResult.isLoading || runCycleResult.isLoading;
    const error = runCycleResult.error ?? runCycleResult.error ?? handleChoiceError;

    const colony = myColonyResult?.data?.data;
    const episode = episodeActionResult?.data?.data ?? runCycleResult?.data;

    useEffect(() => {
        runCycleMutation();
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleSetSlideId = (slideId: string) => {
        const index = episode?.slides.findIndex(x => x.id == slideId);
        if (index == undefined)
            return;
        setSlideIndex(index);
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

    const handleInputTextSave = async () => {
        setInputTextValue(SanitizeInpitText(inputTextValue));
        const validationResult = ValidateInpitText(inputTextValue);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        }
        else
            handleEpisodeAction({ actionName: 'SetChoice', actionParameters: inputTextValue });
    };
    
    const handleEpisodeAction = async (action: SlideButtonAction) => {
        try {
            const response = await episodeActionMutation({ actionName: action.actionName, actionParameters: action.actionParameters }).unwrap();
            if (response.data == undefined)
                navigate('/me/colony');
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setHandleChoiceError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setHandleChoiceError('Неизвестная ошибка.');
            }
        }
    };

    const validateInputText = (value: string): boolean => {
        const validationResult = ValidateInpitText(value);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
            return false;
        }
        setInputTextError('');
        return true;
    };

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (parameters.length == 0)
            return <></>

        return (
            <Box
                display="flex"
                flexDirection="column"
                gap={1}
                sx={{
                    width: '100%',
                    maxWidth: isMobile ? 350 : 700,
                    margin: '0 auto'
                }}
            >
                <ColonyParameterList items={parameters} />
            </Box>
        )
    }

    const renderSlideButton = (button: SlideButton) => {
        const isMutation = button.action != undefined;
        const onClick = button.action != undefined
            ? () => handleEpisodeAction(button.action!)
            : button.navigate != undefined
                ? () => navigate(button.navigate!.actionUrl)
                : button.toSlide != undefined
                    ? () => handleSetSlideId(button.toSlide!.slideId)
                    : () => { };

        return (
            <YagoButton type={isMutation ? 'mutation' : 'navigation'} onClick={onClick} isDisabled={!button.isAvailable}>
                {button.name}
            </YagoButton>)
    }

    const renderPrologueSlide = (slide: Slide) => {
        return (
            <YagoCard
                title={slide.title}
                image={`/assets/images/pictures/${slide.imageName}.jpg`}
            >
                <TextMain textArray={slide.text} />
                {renderParameters(slide.parameters)}
                {slide.textInput != undefined && <YagoCardContentInputField value={inputTextValue} label='Название колонии' handleChange={handleInputTextChange} error={inputTextError} />}
                {slide.buttons.map(x => renderSlideButton(x))}
                {slide.textInput != undefined && <YagoButton onClick={() => handleInputTextSave()} >{slide.continueButtonName}</YagoButton>}
                {renderCloseButton()}
            </YagoCard>
        )
    }

    const renderCloseButton = () => {
        const path = colony?.autoRunCycle ? "/" : "/me/colony";
        return (
            <YagoButton onClick={() => navigate(path)} type='secondary'>Закрыть</YagoButton>
        )
    }

    const renderCard = (episode: Episode) => {
        return renderPrologueSlide(episode.slides[slideIndex]);
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading || episode == undefined
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard(episode)}
        </>
    )
}

export default RunCyclePage
