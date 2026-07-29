import React, { useEffect } from "react";
import Page from "../widgets/Page";
import { useRunCycleMutation } from "../entities/cycles/MyCycle";
import ResultSlideRenderer from "../entities/events/ResultSlideRenderer";
import { useNavigate } from "react-router-dom";


const TurnResultPage: React.FC = () => {
    const navigate = useNavigate();

    const [runCycleMutation, runCycleResult] = useRunCycleMutation();

    useEffect(() => {
        const fetchResult = async () => {
            const result = await runCycleMutation().unwrap();
            if (!result.data) {
                navigate('/me/colony');
            }
        };
        fetchResult();
    }, [runCycleMutation, navigate]);

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error;

    const eventResultSlide = runCycleResult.data?.data;

    const renderContent = () => {
        if (!eventResultSlide) return null;
        return <ResultSlideRenderer eventResult={eventResultSlide} />;
    };

    return (
        <Page backgroundImage="captain_hall" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
}

export default TurnResultPage;
