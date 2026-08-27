import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import { useGetUserPrivateQuery } from "../entities/users/user.api";
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import { useGetReformQuery, useSetReformMutation } from '../entities/reforms/reform.api';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import type { ReformDetails } from '../entities/reforms/reform.types';
import type { Slide, SlideButton } from '../entities/events/colonyEvent.types';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';

const ReformPage: React.FC = () => {
    const { code } = useParams();
    const navigate = useNavigate();

    const userPrivateResult = useGetUserPrivateQuery();
    const myColonyResult = useGetMyColonyQuery();
    const reformResult = useGetReformQuery(code ?? '');
    const [setReform, setReformResult] = useSetReformMutation();
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');

    const isLoading = userPrivateResult.isLoading || myColonyResult.isLoading || reformResult.isLoading || setReformResult.isLoading;
    const error = userPrivateResult.error ?? myColonyResult.error ?? reformResult.error ?? setReformResult.error;

    const reform = reformResult.data;
    const eventResultSlide = setReformResult.data?.data;

    useEffect(() => {
        if (!userPrivateResult.isLoading && !userPrivateResult.data?.data) {
            navigate('/registration');
        }
    }, [userPrivateResult, navigate]);

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data.data == undefined) {
            navigate('/');
        }
    }, [myColonyResult, navigate]);

    useEffect(() => {
        if (!reformResult.isFetching && reformResult.isSuccess && reform == undefined) {
            navigate('/me/reforms');
        }
    }, [reformResult, reform, navigate]);
    
    const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setInputTextValue(value);
        if (value.length > 1) {
            const validationResult = ValidateColonyName(value);
            setInputTextError(validationResult.isValid ? '' : validationResult.error!);
        } else {
            setInputTextError('');
        }
    };

    const buildSlide = (reformDetails: ReformDetails): Slide => ({
        id: reformDetails.code,
        title: reformDetails.name,
        imageName: reformDetails.image,
        text: reformDetails.description,
        visibleEffects: reformDetails.visibleEffects,
        requirements: reformDetails.requirements,
        buttons: [reformDetails.button],
    });

    const handleInputTextSave = async (reformCode: string) => {
        const sanitizedValue = SanitizeColonyName(inputTextValue);
        setInputTextValue(sanitizedValue);
        const validationResult = ValidateColonyName(sanitizedValue);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        } else {
            setInputTextError('');
            await handleSetReform(reformCode, sanitizedValue);
        }
    };

    const handleSetReform = async (reformCode: string, reformValue: string) => {
        const result = await setReform({ reformCode, reformValue }).unwrap();
        if (result.data == undefined || !result.data.show) {
            navigate('/me/colony');
        }
    };

    const handleButtonClick = (button: SlideButton) => {
        if (!button.action || !reform) return;
        if (button.action.type == 'inputCompleted') {
            handleInputTextSave(reform.code);
        } else {
            handleSetReform(reform.code, '');
        }
    };

    const handleCloseResult = () => navigate('/me/reforms');

    const renderContent = () => {
        if (reform == undefined) {
            return null;
        }

        if (eventResultSlide != undefined) {
            return <ResultSlideRenderer eventResult={eventResultSlide} onClose={handleCloseResult} />;
        }

        const leftButton = { icon: ArrowLeft, onClick: () => navigate('/me/reforms'), label: 'Назад' };
        return (
            <SlideRenderer
                slide={buildSlide(reform)}
                actions={{ onButtonClick: handleButtonClick }}
                inputState={{
                    value: inputTextValue,
                    error: inputTextError,
                    onChange: handleInputTextChange,
                }}
                header={{ leftButton: leftButton }}
                resetScrollTrigger={reform.code}
            />
        );
    };

    return (
        <Page backgroundImage='captain_hall' darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ReformPage;