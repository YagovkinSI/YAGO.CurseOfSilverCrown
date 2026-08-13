import React, { useEffect } from "react";
import Page from "../widgets/Page";
import { useRunTurnMutation } from "../entities/turns/MyTurn";
import ResultSlideRenderer from "../entities/events/ResultSlideRenderer";
import { useNavigate } from "react-router-dom";


const TurnResultPage: React.FC = () => {
    const navigate = useNavigate();

    const [runTurnMutation, runTurnResult] = useRunTurnMutation();

    useEffect(() => {
        const fetchResult = async () => {
            const result = await runTurnMutation().unwrap();
            if (!result.data) {
                navigate('/me/colony');
            }
        };
        fetchResult();
    }, [runTurnMutation, navigate]);

    const isLoading = runTurnResult.isLoading;
    const error = runTurnResult.error;

    const eventResultSlide = runTurnResult.data?.data;

    const renderContent = () => {
        if (!eventResultSlide) return null;
        return <ResultSlideRenderer 
            eventResult={eventResultSlide} 
            onClose={() => navigate('/me/colony')}
            />;
    };

    return (
        <Page backgroundImage="captain_hall" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
}

export default TurnResultPage;
