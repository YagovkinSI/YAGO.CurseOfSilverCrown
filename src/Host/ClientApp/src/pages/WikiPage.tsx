import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import type { Slide } from '../entities/events/colonyEvent.types';
import { getWikiArticle } from '../entities/wiki/wiki.data';
import type { WikiArticle } from '../entities/wiki/wiki.types';

const WikiPage: React.FC = () => {
    const { code } = useParams();
    const navigate = useNavigate();

    const article = getWikiArticle(code);

    useEffect(() => {
        if (code != undefined && article == undefined) {
            navigate('/wiki', { replace: true });
        }
    }, [code, article, navigate]);

    const buildSlide = (wikiArticle: WikiArticle): Slide => ({
        id: wikiArticle.code,
        title: wikiArticle.name,
        imageName: wikiArticle.imageName,
        text: wikiArticle.text,
        visibleEffects: [],
        requirements: [],
        buttons: [],
    });

    const renderContent = () => {
        if (article == undefined)
            return null;
        return (
            <SlideRenderer
                slide={buildSlide(article)}
                header={{ leftButton: { icon: ArrowLeft, onClick: () => navigate('/wiki'), label: 'Назад' } }}
                resetScrollTrigger={article.code}
            />
        );
    };

    const isLoading = false;
    return (
        <Page backgroundImage='space' darkenBackground isLoading={isLoading} error={undefined}>
            {renderContent()}
        </Page>
    );
};

export default WikiPage;
