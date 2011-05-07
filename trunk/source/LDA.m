classdef LDA < handle 
    %ako se ne nasljeðuje od handle tada se 
    %pri pridjeljivanju vrijednosti svojstava treba saèuvati objekt 
    %u varijablu (funkcija treba vraèati obj (this))
    
    properties
        Samples, %niz vektora znaèajki
        Classes, %labele razreda za pojedine uzorake
        
        EigenVectors,
        EigenValues,
        
        BetweenScatter, %SB
        WithinScatter   %SW
    end
    
    properties(Access = 'private')
        ClassSubsetIndexes %gdje koji razred poèinje i završava po indeksu u "Classes"
                
        TotalMean,
        MeanPerClass
    end
       
    methods
        
        function this = LDA(samples, classes)
            this.Samples = samples;
            this.Classes = classes;
            
            this.ClassSubsetIndexes = LDA.GetClassSubsetIndexes(classes);
            [this.TotalMean this.MeanPerClass] = this.CalculateMean(samples);        
        end
              
        function Compute(this)
            SB = this.CalculateBetweenScatter();
            SW = this.CalculateWithinScatter();
            
            [eigVectors, eigValues] = eig(SB, SW);
            eigValues = diag(eigValues);
            
            %--------- soritiraj vrijednosti i vektore ---------%
            sortedValues = sort(eigValues,'descend');
            [c, ind] = sort(eigValues,'descend'); %saèuvaj indekse koji pripadaju
            sortedVectors = eigVectors(:,ind); % presloži kolumne u tom redosljedu
        
            this.EigenVectors = sortedVectors;
            this.EigenValues = sortedValues;
            
            this.BetweenScatter = SB;
            this.WithinScatter = SW;
        end
        
        function projectedSamples = Transform(this, samples, numOfDiscriminants)
            vectors = this.EigenVectors(:, 1:numOfDiscriminants);
            
            %transformirani uzorak je skalar (projekcija na pravcu)
            projectedSamples = samples * vectors; 
        end
        
        function measure = CalculateFLDMeasure(this, numOfDiscriminants)
            SB = this.BetweenScatter;
            SW = this.WithinScatter;
           
            vectors = this.EigenVectors(:, 1:numOfDiscriminants);
            
            measure = det(vectors' * SB * vectors) / det(vectors' * SW * vectors);
        end
        
    end
    
    methods(Access = 'private')
        function [totalMean meanPerClass] = CalculateMean(this, samples)
            
            for classIdx=1 : 1 : length(this.ClassSubsetIndexes)
                startIdx = this.ClassSubsetIndexes(classIdx, 1);
                endIdx = this.ClassSubsetIndexes(classIdx, 2);
                meanPerClass(classIdx, :) = mean( samples(startIdx:endIdx, :), 1);
            end
            
            totalMean = mean(meanPerClass, 1); %ukupna srednja vrijednost
        end

        function SW = CalculateWithinScatter(this)
            featureLength = size(this.Samples, 2);
            SW = zeros(featureLength, featureLength);
            
            for classIdx=1 : 1 : length(this.ClassSubsetIndexes)
                startIdx = this.ClassSubsetIndexes(classIdx, 1);
                endIdx = this.ClassSubsetIndexes(classIdx, 2);
                
                classSamples = this.Samples(startIdx:endIdx, :);
                classMean = this.MeanPerClass(classIdx, :);
                Sw_Class = LDA.CalculateScatterMatrix(classSamples, classMean);
                
                SW = SW + Sw_Class;
            end
        end
        
        function SB = CalculateBetweenScatter(this)
            featureLength = size(this.Samples, 2);
            SB = zeros(featureLength, featureLength);
            
             for classIdx=1 : 1 : length(this.ClassSubsetIndexes)
                 
                 startIdx = this.ClassSubsetIndexes(classIdx, 1);
                 endIdx = this.ClassSubsetIndexes(classIdx, 2);
                 numberOfSamplesInClass = endIdx - startIdx + 1;
                 
                 classMean = this.MeanPerClass(classIdx, :);
                 
                 %jer mi je vektor red
                 Sb_class = (classMean - this.TotalMean)' * (classMean - this.TotalMean); 
                 Sb_class = numberOfSamplesInClass * Sb_class;
                 
                 SB = SB + Sb_class;
             end
            
        end
        
    end
    
    methods(Static, Access = 'private')
        
        function subset = GetClassSubsetIndexes(classes)
            
            subset=[];
            oldClassLabel = 'nekaLabela';
            
            for i=1 : 1 : length(classes)
                if oldClassLabel ~= classes{i}
                    oldClassLabel = classes{i};
                    subset = cat(1, subset, i);
                end
            end
            
            %sada stavi završne indekse
            for i=2 : 1 : size(subset,1)
                endIndex = subset(i, 1);
                subset(i-1, 2) = endIndex-1;
            end
            subset(size(subset,1), 2) = length(classes);           
        end
        
        function Sw_class = CalculateScatterMatrix(classSamples, classMean)
            
            featureLength = size(classSamples, 2);
            Sw_class = zeros(featureLength, featureLength);
            
            for sampleIdx=1 : 1 : length(classSamples)
                covariance = (classSamples(sampleIdx, :) - classMean);
                covariance = covariance' * covariance; %jer mi je vektor red
                
                Sw_class = Sw_class + covariance;
            end          
        end
        
    end
    
end