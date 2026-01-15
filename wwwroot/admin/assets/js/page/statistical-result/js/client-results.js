'use strict';
(function () {
    // Store global data
    let profileData = null;

    //API load profile
    function loadProfile() {
        $.ajax({
            url: systemURL + "profile/api/statistics/1002",
            method: "GET",
            success: function (responseData) {
                if (responseData && responseData.resources) {
                    profileData = responseData.resources;
                    renderProfileData(profileData);
                    updateChartsWithData(profileData);
                }
            },
            error: function (e) {
                console.error('Error loading profile:', e);
            },
        });
    }

    // Render profile data to UI
    function renderProfileData(data) {
        const profile = data.profile || {};
        const metricCards = data.metricCards || [];

        // Update user info
        const userAvatar = document.querySelector('.sidebar-column img.rounded-circle');
        const userName = document.querySelector('.sidebar-column .fs-16px.fw-bold');
        if (userAvatar && profile.avatar) {
            userAvatar.src = profile.avatar || '/images/userdefault.jpg';
        }
        if (userName && profile.accountName) {
            userName.textContent = profile.accountName;
        }

        // Update probability percent
        const probabilityEl = document.querySelector('.bg-gray-300 .fs-26px.text-primary');
        if (probabilityEl && profile.probabilityPercent !== undefined) {
            probabilityEl.textContent = Math.round(profile.probabilityPercent) + '%';
        }

        // Render all metric cards dynamically
        renderAllHealthMetrics(metricCards);

        // Update evaluation
        const evaluationBtn = document.querySelector('.btn-success.bg-green-gradient');
        if (evaluationBtn && profile.riskLevel) {
            const riskLevelMap = {
                'good': { text: 'Tốt', class: 'btn-success bg-green-gradient' },
                'fair': { text: 'Trung bình', class: 'btn-warning bg-warning' },
                'poor': { text: 'Kém', class: 'btn-danger bg-red-gradient' }
            };
            const level = riskLevelMap[profile.riskLevel] || riskLevelMap['good'];
            evaluationBtn.textContent = level.text;
            evaluationBtn.className = 'btn ' + level.class;
        }
    }

    // Helper: Find metric by key
    function findMetric(metricCards, key) {
        return metricCards.find(m => m.metricKey === key);
    }

    // Determine if metric should use reverse mode (higher is better)
    function isReverseMetric(metricKey) {
        const reverseMetrics = ['hdl', 'egfr']; // Metrics where higher is better
        return reverseMetrics.includes(metricKey?.toLowerCase());
    }

    // Get default thresholds for a metric if not found in API
    function getDefaultThresholds(metricKey) {
        const defaults = {
            'total_cholesterol': { low: 0, medium: 200, high: 240 },
            'hdl': { low: 0, medium: 40, high: 60 },
            'ldl': { low: 0, medium: 100, high: 160 },
            'tg': { low: 0, medium: 150, high: 200 },
            'systolic_bp': { low: 0, medium: 120, high: 140 },
            'blood_pressure_systolic': { low: 0, medium: 120, high: 140 },
            'glucose_fasting': { low: 0, medium: 100, high: 126 },
            'hba1c': { low: 0, medium: 5.7, high: 6.5 }
        };
        return defaults[metricKey?.toLowerCase()] || { low: 0, medium: 100, high: 200 };
    }

    // Render all health metrics dynamically
    function renderAllHealthMetrics(metricCards) {
        if (!metricCards || metricCards.length === 0) return;

        // Find container to insert metrics
        const healthProfileCard = document.querySelector('.sidebar-column .card-custom:last-child .card-body');
        if (!healthProfileCard) return;

        // Find the container where health metrics should be inserted
        const metricsContainer = healthProfileCard.querySelector('.d-flex.flex-column.w-100.gap-5');
        if (!metricsContainer) return;

        // Remove existing health containers (keep button and evaluation sections)
        const existingContainers = metricsContainer.querySelectorAll('.health-container');
        existingContainers.forEach(container => container.remove());

        // Get the button to insert after
        const healthProfileBtn = metricsContainer.querySelector('a.btn-primary');
        if (!healthProfileBtn) return;

        // Filter metrics that have numeric values (skip text-only metrics for now)
        const numericMetrics = metricCards.filter(m => m.valueNumeric != null && m.valueNumeric !== undefined);

        // Add total cholesterol if not present but can be calculated
        const hasTotalChol = numericMetrics.find(m => m.metricKey === 'total_cholesterol');
        if (!hasTotalChol) {
            const totalChol = calculateTotalCholesterol(metricCards);
            if (totalChol) {
                // Try to get thresholds from total_cholesterol metric if exists
                const totalCholMetric = findMetric(metricCards, 'total_cholesterol');
                numericMetrics.unshift({
                    metricKey: 'total_cholesterol',
                    name: 'Cholesterol toàn phần',
                    thresholds: totalCholMetric?.thresholds || [],
                    ...totalChol
                });
            }
        }

        // Render each metric
        numericMetrics.forEach((metric, index) => {
            const container = createHealthMetricContainer(metric, index);
            if (container) {
                // Insert after the button
                healthProfileBtn.insertAdjacentElement('afterend', container);
            }
        });

        // Update each metric with data after a short delay to ensure DOM is ready
        setTimeout(() => {
            numericMetrics.forEach((metric, index) => {
                const isReverse = isReverseMetric(metric.metricKey);
                let thresholds = null;
                
                if (metric.thresholds && metric.thresholds.length > 0) {
                    thresholds = parseThresholdsFromAPI(metric.thresholds, isReverse);
                }
                
                // Fallback to default if no thresholds found
                if (!thresholds) {
                    const defaults = getDefaultThresholds(metric.metricKey);
                    thresholds = { ...defaults, allThresholds: [] };
                }

                updateHealthMetric(metric.name || metric.metricKey, metric, index, {
                    value: metric.valueNumeric,
                    unit: metric.unit || '',
                    statusLabel: metric.statusLabel,
                    statusLevel: metric.statusLevel,
                    thresholds: thresholds,
                    reverse: isReverse
                });
            });
        }, 50);
    }

    // Create health metric container HTML
    function createHealthMetricContainer(metric, index) {
        const container = document.createElement('div');
        container.className = 'health-container d-flex flex-column gap-1';
        
        const value = Math.round(metric.valueNumeric || 0);
        const unit = metric.unit || '';
        const name = metric.name || metric.metricKey;
        const description = metric.description || '';
        const uniqueId = `metric_${metric.metricKey}_${index}_${Date.now()}`;
        
        // Determine color class based on status
        let colorClass = 'bg-orange-gradient';
        if (metric.statusLevel === 'good' || metric.statusLevel === 'success') {
            colorClass = 'bg-green-gradient';
        } else if (metric.statusLevel === 'poor' || metric.statusLevel === 'danger') {
            colorClass = 'bg-red-gradient';
        }

        // Determine segment order (reverse for HDL, etc.)
        const isReverse = isReverseMetric(metric.metricKey);
        const segmentOrder = isReverse 
            ? '<div class="segment red-segment"></div><div class="segment orange-segment"></div><div class="segment green-segment"></div>'
            : '<div class="segment green-segment"></div><div class="segment orange-segment"></div><div class="segment red-segment"></div>';

        // Escape HTML to prevent XSS
        const escapeHtml = (text) => {
            const div = document.createElement('div');
            div.textContent = text;
            return div.innerHTML;
        };

        container.innerHTML = `
            <span class="fw-bold">${escapeHtml(name)}:</span>
            <div class="d-flex flex-row align-items-center gap-2">
                <div class="rounded-1 ${colorClass} fs-18px py-1 px-4">${value}</div>
                <span class="fw-bold">${escapeHtml(metric.statusLabel || 'Chưa xác định')}</span>
            </div>
            <span class="fs-12px">${escapeHtml(description)}</span>
            <div class="figma-risk-widget">
                <div class="risk-bar-container">
                    <div class="risk-progress-bar">
                        ${segmentOrder}
                    </div>
                    <div class="value-indicator" style="left: 50%;">
                        <div class="value-pin">
                            <span>${value}</span>
                            <svg xmlns="http://www.w3.org/2000/svg" width="30" height="36" viewBox="0 0 30 36" fill="none">
                                <mask id="mask_${uniqueId}" fill="white">
                                    <path d="M9.25684 1.14258C12.0005 0.00547218 15.02 -0.291576 17.9326 0.289062C20.8456 0.869857 23.5212 2.30257 25.6191 4.40527C28.4252 7.22692 30 11.045 30 15.0244C29.9999 19.0036 28.425 22.821 25.6191 25.6426L15.6279 35.6338C15.5455 35.7162 15.4475 35.7816 15.3398 35.8262C15.2321 35.8708 15.1166 35.8945 15 35.8945C14.8834 35.8945 14.7679 35.8708 14.6602 35.8262C14.5525 35.7816 14.4545 35.7162 14.3721 35.6338L4.38086 25.6426C2.98492 24.2503 1.87894 22.595 1.12695 20.7725C0.374936 18.9498 -0.00804686 16.9961 0 15.0244C-0.00480055 12.0541 0.872598 9.14896 2.52051 6.67773C4.16841 4.20661 6.513 2.27987 9.25684 1.14258Z" />
                                </mask>
                                <path d="M9.25684 1.14258C12.0005 0.00547218 15.02 -0.291576 17.9326 0.289062C20.8456 0.869857 23.5212 2.30257 25.6191 4.40527C28.4252 7.22692 30 11.045 30 15.0244C29.9999 19.0036 28.425 22.821 25.6191 25.6426L15.6279 35.6338C15.5455 35.7162 15.4475 35.7816 15.3398 35.8262C15.2321 35.8708 15.1166 35.8945 15 35.8945C14.8834 35.8945 14.7679 35.8708 14.6602 35.8262C14.5525 35.7816 14.4545 35.7162 14.3721 35.6338L4.38086 25.6426C2.98492 24.2503 1.87894 22.595 1.12695 20.7725C0.374936 18.9498 -0.00804686 16.9961 0 15.0244C-0.00480055 12.0541 0.872598 9.14896 2.52051 6.67773C4.16841 4.20661 6.513 2.27987 9.25684 1.14258Z" fill="url(#grad_${uniqueId})" />
                                <path d="M9.25684 1.14258L8.6846 -0.238126L8.68456 -0.238107L9.25684 1.14258ZM17.9326 0.289062L18.2249 -1.17668L18.2248 -1.17669L17.9326 0.289062ZM25.6191 4.40527L26.6789 3.35137L26.6772 3.34965L25.6191 4.40527ZM30 15.0244L31.4946 15.0244V15.0244H30ZM25.6191 25.6426L26.676 26.6994L26.6789 26.6965L25.6191 25.6426ZM15.6279 35.6338L14.5711 34.577L14.571 34.577L15.6279 35.6338ZM15.3398 35.8262L15.9118 37.207L15.9121 37.2069L15.3398 35.8262ZM15 35.8945L15 37.3891H15V35.8945ZM14.6602 35.8262L14.0878 37.2068L14.0882 37.207L14.6602 35.8262ZM14.3721 35.6338L15.429 34.577L15.4289 34.577L14.3721 35.6338ZM4.38086 25.6426L5.43769 24.5857L5.43631 24.5844L4.38086 25.6426ZM1.12695 20.7725L-0.254655 21.3425L-0.254652 21.3425L1.12695 20.7725ZM0 15.0244L1.4946 15.0305L1.49459 15.022L0 15.0244ZM2.52051 6.67773L1.27705 5.84852L1.27703 5.84854L2.52051 6.67773ZM9.25684 1.14258L9.82907 2.52328C12.2993 1.49948 15.018 1.23202 17.6404 1.75481L17.9326 0.289062L18.2248 -1.17669C15.022 -1.81518 11.7016 -1.48853 8.6846 -0.238126L9.25684 1.14258ZM17.9326 0.289062L17.6404 1.7548C20.2631 2.27774 22.6723 3.56775 24.5611 5.4609L25.6191 4.40527L26.6772 3.34965C24.3702 1.03739 21.428 -0.538026 18.2249 -1.17668L17.9326 0.289062ZM25.6191 4.40527L24.5594 5.45918C27.0869 8.00068 28.5054 11.4398 28.5054 15.0244H30H31.4946C31.4946 10.6502 29.7636 6.45316 26.6789 3.35137L25.6191 4.40527ZM30 15.0244L28.5054 15.0244C28.5053 18.6087 27.0867 22.0472 24.5594 24.5887L25.6191 25.6426L26.6789 26.6965C29.7633 23.5948 31.4945 19.3986 31.4946 15.0244L30 15.0244ZM25.6191 25.6426L24.5623 24.5857L14.5711 34.577L15.6279 35.6338L16.6848 36.6906L26.676 26.6994L25.6191 25.6426ZM15.6279 35.6338L14.571 34.577C14.6282 34.5198 14.6951 34.4755 14.7676 34.4455L15.3398 35.8262L15.9121 37.2069C16.1999 37.0876 16.4628 36.9126 16.6848 36.6906L15.6279 35.6338ZM15.3398 35.8262L14.7678 34.4454C14.8354 34.4174 14.9139 34.3999 15 34.3999V35.8945V37.3891C15.3193 37.3891 15.6288 37.3242 15.9118 37.207L15.3398 35.8262ZM15 35.8945L15 34.3999C15.0861 34.3999 15.1645 34.4174 15.2322 34.4454L14.6602 35.8262L14.0882 37.207C14.3712 37.3242 14.6807 37.3891 15 37.3891L15 35.8945ZM14.6602 35.8262L15.2325 34.4455C15.3049 34.4755 15.3717 34.5198 15.429 34.577L14.3721 35.6338L13.3152 36.6906C13.5372 36.9126 13.8002 37.0876 14.0878 37.2068L14.6602 35.8262ZM14.3721 35.6338L15.4289 34.577L5.43769 24.5857L4.38086 25.6426L3.32403 26.6994L13.3152 36.6906L14.3721 35.6338ZM4.38086 25.6426L5.43631 24.5844C4.18028 23.3316 3.18517 21.8423 2.50856 20.2024L1.12695 20.7725L-0.254652 21.3425C0.572718 23.3478 1.78955 25.169 3.32541 26.7008L4.38086 25.6426ZM1.12695 20.7725L2.50856 20.2024C1.83194 18.5625 1.48734 16.8046 1.49458 15.0305L0 15.0244L-1.49458 15.0183C-1.50343 17.1876 -1.08207 19.3371 -0.254655 21.3425L1.12695 20.7725ZM0 15.0244L1.49459 15.022C1.49027 12.3477 2.28024 9.73197 3.76398 7.50693L2.52051 6.67773L1.27703 5.84854C-0.535042 8.56595 -1.49987 11.7606 -1.49459 15.0268L0 15.0244ZM2.52051 6.67773L3.76397 7.50695C5.24773 5.28197 7.35876 3.5472 9.82912 2.52326L9.25684 1.14258L8.68456 -0.238107C5.66725 1.01254 3.08909 3.13125 1.27705 5.84852L2.52051 6.67773Z" fill="white" mask="url(#mask_${uniqueId})" />
                                <defs>
                                    <linearGradient id="grad_${uniqueId}" x1="30.0001" y1="11.334" x2="4.83206" y2="26.1688" gradientUnits="userSpaceOnUse">
                                        <stop stop-color="#FEAC52" />
                                        <stop offset="1" stop-color="#F95638" />
                                    </linearGradient>
                                </defs>
                            </svg>
                        </div>
                        <div class="value-circle"></div>
                    </div>
                </div>
                <div class="thresholds">
                    <div class="threshold-item" style="left: 33.333333%;">
                        <div class="tick-mark bg-success"></div>
                        <span class="threshold-text green-text">-</span>
                    </div>
                    <div class="threshold-item" style="left: 66.666666%;">
                        <div class="tick-mark bg-warning"></div>
                        <span class="threshold-text orange-text">-</span>
                    </div>
                </div>
                <div class="separator my-3"></div>
            </div>
        `;
        
        return container;
    }

    // Parse thresholds from API and calculate low, medium, high for display
    function parseThresholdsFromAPI(thresholds, reverse = false) {
        if (!thresholds || thresholds.length === 0) {
            return null;
        }

        // Sort by priority
        const sorted = [...thresholds].sort((a, b) => (a.priority || 0) - (b.priority || 0));

        if (reverse) {
            // For HDL: higher is better
            // Find thresholds: low (bad) and good threshold
            const lowThreshold = sorted.find(t => 
                t.thresholdType?.includes('low') || 
                (t.maxValue != null && t.minValue == null)
            );
            const goodThreshold = sorted.find(t => 
                t.thresholdType === 'male' || 
                t.thresholdType === 'female' || 
                t.thresholdType?.includes('normal') || 
                t.thresholdType?.includes('good') || 
                (t.minValue != null && t.maxValue == null && !t.thresholdType?.includes('low'))
            );
            
            // Use good threshold minValue as the "good" threshold
            // Use low threshold maxValue as the "low" threshold
            const lowValue = lowThreshold?.maxValue || 0;
            const goodValue = goodThreshold?.minValue || (lowThreshold?.minValue || 40);
            const highValue = goodThreshold?.minValue || (goodThreshold?.maxValue || 60);
            
            return {
                low: lowValue,
                medium: goodValue,
                high: highValue,
                allThresholds: sorted
            };
        } else {
            // Normal: lower is better
            // Find normal range and high threshold
            const normalThreshold = sorted.find(t => 
                t.thresholdType?.includes('normal') || 
                (t.minValue != null && t.maxValue != null && !t.thresholdType?.includes('high'))
            );
            const highThreshold = sorted.find(t => 
                t.thresholdType?.includes('high') || 
                t.thresholdType?.includes('elevated') || 
                t.thresholdType?.includes('diabetes') ||
                (t.minValue != null && t.maxValue == null)
            );
            
            const normalHigh = normalThreshold?.maxValue;
            const highValue = highThreshold?.minValue;
            
            return {
                low: 0,
                medium: normalHigh || (highValue ? highValue * 0.8 : 200),
                high: highValue || (normalHigh ? normalHigh * 1.2 : 240),
                allThresholds: sorted
            };
        }
    }

    // Calculate total cholesterol from HDL + LDL + TG/5
    function calculateTotalCholesterol(metricCards) {
        const hdl = findMetric(metricCards, 'hdl');
        const ldl = findMetric(metricCards, 'ldl');
        const tg = findMetric(metricCards, 'tg');

        if (hdl && ldl && tg && hdl.valueNumeric && ldl.valueNumeric && tg.valueNumeric) {
            const total = hdl.valueNumeric + ldl.valueNumeric + (tg.valueNumeric / 5);
            return {
                valueNumeric: Math.round(total),
                unit: 'mg/dL',
                statusLabel: getCholesterolStatus(total),
                statusLevel: getCholesterolLevel(total)
            };
        }
        return null;
    }

    // Get cholesterol status label
    function getCholesterolStatus(value) {
        if (value < 200) return 'Ngưỡng bình thường';
        if (value < 240) return 'Ngưỡng cận cao';
        return 'Ngưỡng cao';
    }

    // Get cholesterol level
    function getCholesterolLevel(value) {
        if (value < 200) return 'good';
        if (value < 240) return 'fair';
        return 'poor';
    }

    // Update health metric display
    function updateHealthMetric(metricName, metric, index, config) {
        // Find container by metric name
        const healthContainers = document.querySelectorAll('.health-container');
        let container = null;
        
        // Try to find by metric name in the first fw-bold span
        for (let i = 0; i < healthContainers.length; i++) {
            const titleSpan = healthContainers[i].querySelector('.fw-bold');
            if (titleSpan && titleSpan.textContent.includes(metricName)) {
                container = healthContainers[i];
                break;
            }
        }
        
        // Fallback to index if not found
        if (!container && healthContainers[index]) {
            container = healthContainers[index];
        }
        
        if (!container) return;
        const valueEl = container.querySelector('.rounded-1');
        const statusEl = container.querySelector('.fw-bold:not(.health-container > .fw-bold)');
        const descriptionEl = container.querySelector('.fs-12px');
        const valueIndicator = container.querySelector('.value-indicator');
        const valuePin = container.querySelector('.value-pin span');

        // Update value
        if (valueEl) {
            const value = Math.round(config.value);
            valueEl.textContent = value;
            
            // Update color based on status
            valueEl.className = 'rounded-1 fs-18px py-1 px-4';
            if (config.statusLevel === 'good') {
                valueEl.classList.add('bg-green-gradient');
            } else if (config.statusLevel === 'fair' || config.statusLevel === 'neutral') {
                valueEl.classList.add('bg-orange-gradient');
            } else {
                valueEl.classList.add('bg-red-gradient');
            }
        }

        // Update status label
        if (statusEl && config.statusLabel) {
            statusEl.textContent = config.statusLabel;
        }

        // Update description if available
        if (descriptionEl && metric.description) {
            descriptionEl.textContent = metric.description;
        }

        // Update value indicator position and pin
        if (valueIndicator && config.thresholds) {
            const { low, medium, high } = config.thresholds;
            const max = high || (medium * 1.5);
            let percent = 0;

            if (config.reverse) {
                // For HDL: higher is better
                if (config.value >= high) {
                    percent = 80; // Top range
                } else if (config.value >= medium) {
                    percent = 50; // Middle range
                } else {
                    percent = 20; // Low range
                }
            } else {
                // Normal calculation
                if (config.value < medium) {
                    percent = (config.value / medium) * 33.33;
                } else if (config.value < high) {
                    percent = 33.33 + ((config.value - medium) / (high - medium)) * 33.33;
                } else {
                    percent = 66.66 + Math.min(33.33, ((config.value - high) / (max - high)) * 33.33);
                }
            }

            percent = Math.max(0, Math.min(100, percent));
            valueIndicator.style.left = percent + '%';

            // Update pin value
            if (valuePin) {
                valuePin.textContent = Math.round(config.value);
            }

            // Update threshold labels from API data
            const thresholdsContainer = container.querySelector('.thresholds');
            if (thresholdsContainer && config.thresholds && config.thresholds.allThresholds) {
                const thresholdItems = thresholdsContainer.querySelectorAll('.threshold-item');
                const unit = config.unit || '';
                const allThresholds = config.thresholds.allThresholds;
                
                // Find relevant thresholds for display (typically 2 thresholds)
                let displayThresholds = [];
                
                if (config.reverse) {
                    // For HDL: show low threshold and good threshold
                    const lowThresh = allThresholds.find(t => t.thresholdType?.includes('low') || t.maxValue != null);
                    const goodThresh = allThresholds.find(t => t.thresholdType?.includes('normal') || t.thresholdType?.includes('good') || t.thresholdType?.includes('male') || t.thresholdType?.includes('female'));
                    if (lowThresh) displayThresholds.push({ value: lowThresh.maxValue, label: `<${Math.round(lowThresh.maxValue)} ${unit}`, position: 33.33 });
                    if (goodThresh) displayThresholds.push({ value: goodThresh.minValue, label: `>${Math.round(goodThresh.minValue)} ${unit}`, position: 66.66 });
                } else {
                    // Normal: show normal upper bound and high threshold
                    const normalThresh = allThresholds.find(t => t.thresholdType?.includes('normal') || (t.minValue != null && t.maxValue != null));
                    const highThresh = allThresholds.find(t => t.thresholdType?.includes('high') || t.thresholdType?.includes('elevated') || (t.minValue != null && t.maxValue == null));
                    if (normalThresh) displayThresholds.push({ value: normalThresh.maxValue, label: `<${Math.round(normalThresh.maxValue)} ${unit}`, position: 33.33 });
                    if (highThresh) displayThresholds.push({ value: highThresh.minValue, label: `>${Math.round(highThresh.minValue)} ${unit}`, position: 66.66 });
                }
                
                // Fallback to calculated thresholds if no specific thresholds found
                if (displayThresholds.length === 0) {
                    displayThresholds = [
                        { value: config.thresholds.medium, label: `<${Math.round(config.thresholds.medium)} ${unit}`, position: 33.33 },
                        { value: config.thresholds.high, label: `>${Math.round(config.thresholds.high)} ${unit}`, position: 66.66 }
                    ];
                }
                
                // Update threshold items
                if (thresholdItems.length >= 2 && displayThresholds.length >= 2) {
                    // Update first threshold
                    const firstItem = thresholdItems[0];
                    const firstText = firstItem.querySelector('.threshold-text');
                    if (firstText) {
                        firstText.textContent = displayThresholds[0].label;
                    }
                    firstItem.style.left = displayThresholds[0].position + '%';
                    
                    // Update second threshold
                    const secondItem = thresholdItems[1];
                    const secondText = secondItem.querySelector('.threshold-text');
                    if (secondText) {
                        secondText.textContent = displayThresholds[1].label;
                    }
                    secondItem.style.left = displayThresholds[1].position + '%';
                }
            }

            // Update pointer color after position change
            setTimeout(() => {
                updateRiskPointerColor();
            }, 100);
        }
    }

    // Update charts with data
    function updateChartsWithData(data) {
        const profile = data.profile || {};
        const riskMarkers = data.riskMarkers || [];
        const riskTrend = data.riskTrend || [];
        const riskTrendMarker = data.riskTrendMarker || {};

        // Update genetic risk donut chart
        if (window.setRiskLevel && profile.riskLevel) {
            const riskLevelMap = {
                'good': 'green',
                'fair': 'orange',
                'poor': 'red'
            };
            const level = riskLevelMap[profile.riskLevel] || 'orange';
            window.setRiskLevel(level);
        }

        // Update risk bar chart
        if (window.setRiskBar && riskMarkers.length > 0) {
            const markers = riskMarkers.map(m => ({
                value: m.value,
                label: String(m.value),
                type: m.type === 'clinical' ? 1001 : 1002
            }));

            // Calculate thresholds based on max value
            const maxValue = Math.max(...riskMarkers.map(m => m.value), 24);
            const thresholds = {
                greenEnd: maxValue * 0.33,
                orangeEnd: maxValue * 0.67
            };

            window.setRiskBar(thresholds, markers);
        }

        // Update risk trend chart
        if (window.riskTrendChart && riskTrend.length > 0) {
            updateRiskTrendChart(riskTrend, riskTrendMarker);
        } else if (riskTrend.length > 0) {
            // If chart not initialized yet, reinitialize with new data
            const chartCanvas = window.chartCanvasInstance;
            if (chartCanvas) {
                // Destroy existing chart if it exists
                if (window.riskTrendChart) {
                    window.riskTrendChart.destroy();
                    window.riskTrendChart = null;
                }
                chartCanvas.initChartRiskOfDevelopingDisease(riskTrend, riskTrendMarker);
            }
        }
    }

    // Update risk trend chart
    function updateRiskTrendChart(riskTrend, riskTrendMarker) {
        if (!window.riskTrendChart || !riskTrend || riskTrend.length === 0) return;

        const ages = riskTrend.map(item => item.age);
        const currentRisk = riskTrend.map(item => item.currentRisk || 0);
        const lifestyleRisk = riskTrend.map(item => item.lifestyleRisk || 0);
        const averageRisk = riskTrend.map(item => item.averageRisk || 0);

        // Update chart data
        window.riskTrendChart.data.labels = ages;
        window.riskTrendChart.data.datasets[0].data = currentRisk;
        window.riskTrendChart.data.datasets[1].data = lifestyleRisk;
        window.riskTrendChart.data.datasets[2].data = averageRisk;
        window.riskTrendChart.data.datasets[3].data = averageRisk;
        window.riskTrendChart.data.datasets[4].data = averageRisk;
        window.riskTrendChart.data.datasets[5].data = currentRisk;

        // Update Y axis max
        const allValues = [...currentRisk, ...lifestyleRisk, ...averageRisk];
        const dataMax = Math.max(...allValues);
        const yAxisMax = Math.ceil(dataMax / 5) * 5 + 5;
        window.riskTrendChart.options.scales.y.max = yAxisMax;

        // Update user marker data
        // Store user marker data in window object for access
        if (riskTrendMarker && window.riskTrendChart) {
            // Find the plugin and update its closure data
            // Since we can't directly access closure, we'll store it in window object
            if (!window.userMarkerData) {
                window.userMarkerData = {};
            }
            window.userMarkerData.age = riskTrendMarker.age;
            window.userMarkerData.risk = riskTrendMarker.risk || 
                currentRisk[ages.findIndex(a => a === riskTrendMarker.age)] || currentRisk[0];
        }

        window.riskTrendChart.update();
    }


    function updateRiskPointerColor() {
        document.querySelectorAll('.risk-bar-container').forEach(container => {
            const indicator = container.querySelector('.value-indicator');
            const circle = indicator?.querySelector('.value-circle');
            const valuePin = indicator?.querySelector('.value-pin');
            const segments = container.querySelectorAll('.risk-progress-bar .segment');

            if (!indicator || !circle || !valuePin || segments.length === 0) return;

            // Lấy phần trăm vị trí hiện tại
            const leftPercent = parseFloat(indicator.style.left) || 0;
            const step = 100 / segments.length;
            const segmentIndex = Math.min(Math.floor(leftPercent / step), segments.length - 1);
            const seg = segments[segmentIndex];

            // Xác định màu gradient
            let gradientStart = '#3D9C6F';
            let gradientEnd = '#69EC9F';

            if (seg.classList.contains('orange-segment')) {
                gradientStart = '#FEAC52';
                gradientEnd = '#F95638';
            } else if (seg.classList.contains('red-segment')) {
                gradientStart = '#CE3853';
                gradientEnd = '#E45585';
            }

            // ✅ Cập nhật màu vòng tròn
            circle.style.background = `linear-gradient(127deg, ${gradientStart}, ${gradientEnd})`;
            circle.style.transition = 'background 0.3s ease';

            // ✅ Cập nhật SVG bên trong .value-pin mà không xóa span
            const oldSvg = valuePin.querySelector('svg');
            const newSvg = getPinSvg(gradientStart, gradientEnd); // 👉 hàm này trả về SVG template dạng chuỗi

            if (oldSvg) {
                oldSvg.outerHTML = newSvg; // chỉ thay SVG
            } else {
                valuePin.insertAdjacentHTML('beforeend', newSvg);
            }
        });
    }
    function getPinSvg(startColor, endColor) {
        const gradientId = "paint_" + Math.random().toString(36).substring(2, 8);
        const maskId = "mask_" + Math.random().toString(36).substring(2, 8);

        return `
        <svg xmlns="http://www.w3.org/2000/svg" width="30" height="36" viewBox="0 0 30 36" fill="none" style="transition: fill 0.3s;">
            <mask id="${maskId}" fill="white">
                <path d="M9.25684 1.14258C12.0005 0.00547218 15.02 -0.291576 17.9326 0.289062C20.8456 0.869857 23.5212 2.30257 25.6191 4.40527C28.4252 7.22692 30 11.045 30 15.0244C29.9999 19.0036 28.425 22.821 25.6191 25.6426L15.6279 35.6338C15.5455 35.7162 15.4475 35.7816 15.3398 35.8262C15.2321 35.8708 15.1166 35.8945 15 35.8945C14.8834 35.8945 14.7679 35.8708 14.6602 35.8262C14.5525 35.7816 14.4545 35.7162 14.3721 35.6338L4.38086 25.6426C2.98492 24.2503 1.87894 22.595 1.12695 20.7725C0.374936 18.9498 -0.00804686 16.9961 0 15.0244C-0.00480055 12.0541 0.872598 9.14896 2.52051 6.67773C4.16841 4.20661 6.513 2.27987 9.25684 1.14258Z"></path>
            </mask>
            
            <!-- Fill chính -->
            <path d="M9.25684 1.14258C12.0005 0.00547218 15.02 -0.291576 17.9326 0.289062C20.8456 0.869857 23.5212 2.30257 25.6191 4.40527C28.4252 7.22692 30 11.045 30 15.0244C29.9999 19.0036 28.425 22.821 25.6191 25.6426L15.6279 35.6338C15.5455 35.7162 15.4475 35.7816 15.3398 35.8262C15.2321 35.8708 15.1166 35.8945 15 35.8945C14.8834 35.8945 14.7679 35.8708 14.6602 35.8262C14.5525 35.7816 14.4545 35.7162 14.3721 35.6338L4.38086 25.6426C2.98492 24.2503 1.87894 22.595 1.12695 20.7725C0.374936 18.9498 -0.00804686 16.9961 0 15.0244C-0.00480055 12.0541 0.872598 9.14896 2.52051 6.67773C4.16841 4.20661 6.513 2.27987 9.25684 1.14258Z"
                fill="url(#${gradientId})"></path>
            
            <!-- Viền trắng -->
             <path d="M9.25684 1.14355L8.68457 -0.237137L8.68456 -0.237131L9.25684 1.14355ZM17.9336 0.290039L18.2259 -1.17569L18.2258 -1.1757L17.9336 0.290039ZM25.6191 4.40527L26.6789 3.35135L26.6772 3.34964L25.6191 4.40527ZM30 15.0244H31.4946V15.0244L30 15.0244ZM25.6191 25.6436L26.676 26.7004L26.6789 26.6975L25.6191 25.6436ZM15.6279 35.6348L16.6845 36.6918L16.6848 36.6916L15.6279 35.6348ZM15.3398 35.8271L15.9116 37.2081L15.9118 37.208L15.3398 35.8271ZM15 35.8945L14.9998 37.3891H15V35.8945ZM14.6602 35.8271L14.088 37.2079L14.0882 37.208L14.6602 35.8271ZM14.3721 35.6348L15.4289 34.5779L15.4289 34.5779L14.3721 35.6348ZM4.38086 25.6436L5.43769 24.5867L5.43629 24.5853L4.38086 25.6436ZM1.12695 20.7725L-0.254668 21.3425L-0.254655 21.3425L1.12695 20.7725ZM0 15.0244L1.4946 15.0305L1.49459 15.022L0 15.0244ZM2.52051 6.67773L1.27706 5.84849L1.27703 5.84854L2.52051 6.67773ZM9.25684 1.14355L9.8291 2.52425C12.2996 1.5003 15.0187 1.23286 17.6413 1.75578L17.9336 0.290039L18.2258 -1.1757C15.0227 -1.81435 11.7019 -1.48773 8.68457 -0.237137L9.25684 1.14355ZM17.9336 0.290039L17.6413 1.75577C20.264 2.27879 22.6725 3.56805 24.5611 5.46091L25.6191 4.40527L26.6772 3.34964C24.3702 1.03745 21.4286 -0.53702 18.2259 -1.17569L17.9336 0.290039ZM25.6191 4.40527L24.5594 5.45919C27.0869 8.00066 28.5054 11.4398 28.5054 15.0244L30 15.0244L31.4946 15.0244C31.4946 10.6502 29.7636 6.45316 26.6789 3.35135L25.6191 4.40527ZM30 15.0244H28.5054C28.5054 18.6091 27.0869 22.0481 24.5594 24.5896L25.6191 25.6436L26.6789 26.6975C29.7637 23.5957 31.4946 19.3987 31.4946 15.0244H30ZM25.6191 25.6436L24.5623 24.5867L14.5711 34.5779L15.6279 35.6348L16.6848 36.6916L26.676 26.7004L25.6191 25.6436ZM15.6279 35.6348L14.5713 34.5777C14.628 34.521 14.6947 34.4766 14.7678 34.4463L15.3398 35.8271L15.9118 37.208C16.2002 37.0885 16.463 36.9132 16.6845 36.6918L15.6279 35.6348ZM15.3398 35.8271L14.7681 34.4462C14.8432 34.4151 14.922 34.3999 15 34.3999V35.8945V37.3891C15.3111 37.3891 15.6211 37.3283 15.9116 37.2081L15.3398 35.8271ZM15 35.8945L15.0002 34.3999C15.0777 34.4 15.1566 34.415 15.2322 34.4463L14.6602 35.8271L14.0882 37.208C14.3791 37.3285 14.6892 37.3891 14.9998 37.3891L15 35.8945ZM14.6602 35.8271L15.2323 34.4464C15.3049 34.4765 15.3719 34.5209 15.4289 34.5779L14.3721 35.6348L13.3152 36.6916C13.5371 36.9135 13.8 37.0886 14.088 37.2079L14.6602 35.8271ZM14.3721 35.6348L15.4289 34.5779L5.43769 24.5867L4.38086 25.6436L3.32403 26.7004L13.3152 36.6916L14.3721 35.6348ZM4.38086 25.6436L5.43629 24.5853C4.18036 23.3327 3.18532 21.8427 2.50856 20.2024L1.12695 20.7725L-0.254655 21.3425C0.572643 23.3476 1.78922 25.1697 3.32543 26.7018L4.38086 25.6436ZM1.12695 20.7725L2.50857 20.2024C1.83197 18.5625 1.4873 16.8046 1.49458 15.0305L0 15.0244L-1.49458 15.0183C-1.50348 17.1876 -1.08201 19.3372 -0.254668 21.3425L1.12695 20.7725ZM0 15.0244L1.49459 15.022C1.49028 12.3478 2.28026 9.73194 3.76398 7.50693L2.52051 6.67773L1.27703 5.84854C-0.535029 8.56593 -1.49985 11.7606 -1.49459 15.0268L0 15.0244ZM2.52051 6.67773L3.76395 7.50698C5.24757 5.28229 7.35861 3.54824 9.82912 2.52424L9.25684 1.14355L8.68456 -0.237131C5.66756 1.01339 3.08923 3.13115 1.27706 5.84849L2.52051 6.67773Z" fill="white" mask="url(#${maskId})"/>
            </path>

            <defs>
                <linearGradient id="${gradientId}" x1="30.0001" y1="11.334" x2="4.83206" y2="26.1688" gradientUnits="userSpaceOnUse">
                    <stop stop-color="${startColor}"></stop>
                    <stop offset="1" stop-color="${endColor}"></stop>
                </linearGradient>
            </defs>
        </svg>
    `;
    }
    function renderChart(apiData) {
        window.chartCanvasInstance = {
            initChartGeneticRisk: function () {
                let CURRENT_RISK_LEVEL = 'orange'; // 'red' | 'orange' | 'green'
                let currentPointerRad = null;   // góc hiện vẽ
                let targetPointerRad = null;    // góc mục tiêu theo segment
                let pointerAnimStart = null;    // timestamp animation
                const POINTER_ANIM_MS = 400;    // thời gian animate mũi tên
                const LEVEL_CONFIG = {
                    red: { label: 'Cao', ring: '#ef476f', arrow: '#ef476f', badgeFrom: '#ef476f', badgeTo: '#FF5E3A' },
                    orange: { label: 'Trung bình', ring: '#ff914d', arrow: '#ff914d', badgeFrom: '#ff9f59', badgeTo: '#ff7e3a' },
                    green: { label: 'Tốt', ring: '#33d69f', arrow: '#33d69f', badgeFrom: '#3D9C6F', badgeTo: '#69EC9F' }
                };
                const OUTER_SEGMENTS = [25, 25, 50];      // đỏ, cam, xanh
                // Helper map risk -> index
                const levelToIndex = { red: 0, orange: 1, green: 2 };

                const colors = ['#ef476f', '#ffa94d', '#2fd8a3'];

                const geneticRiskPlugin = {
                    id: 'geneticRiskPlugin',
                    afterDatasetsDraw(chart) {
                        const { ctx, chartArea: { left, right, top, bottom } } = chart;
                        const cx = (left + right) / 2, cy = (top + bottom) / 2;
                        const meta = chart.getDatasetMeta(0).data[0];
                        const outerR = meta.outerRadius, innerR = meta.innerRadius;

                        // Viền trắng bao quanh vòng ngoài (vành đều: rOut/rIn lệch đúng nửa độ dày)
                        ctx.save();
                        const borderW = 3; // tăng dày viền trắng cho kích thước 300px
                        const rOut = outerR + borderW / 2;
                        const rIn = outerR - borderW / 2;
                        ctx.fillStyle = '#ffffff';
                        ctx.beginPath();
                        ctx.arc(cx, cy, rOut, 0, Math.PI * 2);
                        ctx.arc(cx, cy, rIn, 0, Math.PI * 2, true);
                        ctx.fill('evenodd');
                        ctx.restore();

                        // ====== Vòng cam kín cách donut ngoài một khoảng ======
                        const gapPx = 15;           // tăng khoảng cách cho kích thước 300px
                        const ringWidth = 4;        // tăng độ dày vòng cam cho kích thước 300px
                        const ringR = innerR - gapPx - ringWidth / 2; // tâm nét

                        const levelCfg = LEVEL_CONFIG[CURRENT_RISK_LEVEL] || LEVEL_CONFIG.orange;

                        // Vòng cam 360° (không khuyết)
                        ctx.save();
                        ctx.strokeStyle = levelCfg.ring;
                        ctx.lineWidth = ringWidth;
                        ctx.lineCap = 'round';
                        ctx.beginPath();
                        ctx.arc(cx, cy, ringR, 0, Math.PI * 2);
                        ctx.stroke();
                        ctx.restore();

                        // Tính góc mục tiêu tại TÂM segment theo CURRENT_RISK_LEVEL
                        const levelIndex = levelToIndex[CURRENT_RISK_LEVEL] ?? 0;
                        const arcEl = chart.getDatasetMeta(0).data[levelIndex];
                        targetPointerRad = (arcEl.startAngle + arcEl.endAngle) / 2;

                        // Khởi tạo góc hiện tại lần đầu
                        if (currentPointerRad == null) currentPointerRad = targetPointerRad;

                        // Animate mũi tên tới vị trí mới
                        if (Math.abs(currentPointerRad - targetPointerRad) > 1e-4) {
                            const now = performance.now();
                            if (pointerAnimStart == null) pointerAnimStart = now;
                            const t = Math.min(1, (now - pointerAnimStart) / POINTER_ANIM_MS);
                            currentPointerRad = currentPointerRad + (targetPointerRad - currentPointerRad) * t;
                            // vẽ lại khung tiếp theo
                            requestAnimationFrame(() => chart.draw());
                        } else {
                            pointerAnimStart = null;
                            currentPointerRad = targetPointerRad;
                        }

                        // Vị trí mũi tên: lệch ra ngoài mép vòng cam một chút
                        const arrowOffset = ringWidth / 2;
                        const arrowR = ringR + arrowOffset;
                        const ax = cx + arrowR * Math.cos(currentPointerRad);
                        const ay = cy + arrowR * Math.sin(currentPointerRad);

                        // MŨI TÊN HƯỚNG RA NGOÀI (radial outward)
                        const arrowLen = 10;  // tăng chiều dài mũi tên cho kích thước 300px
                        const arrowW = 12;    // tăng bề ngang mũi tên cho kích thước 300px

                        ctx.save();
                        ctx.translate(ax, ay);
                        ctx.rotate(currentPointerRad);          // cùng hướng bán kính (ra ngoài)
                        ctx.fillStyle = levelCfg.arrow;
                        ctx.strokeStyle = levelCfg.arrow;
                        ctx.lineWidth = 0.6;

                        ctx.beginPath();
                        // đỉnh mũi tên hướng ra ngoài theo trục +x (sau khi rotate)
                        ctx.moveTo(arrowLen, 0);         // tip ra ngoài
                        ctx.lineTo(0, -arrowW / 2);      // cạnh trên
                        ctx.lineTo(0, arrowW / 2);      // cạnh dưới
                        ctx.closePath();
                        ctx.fill();
                        ctx.stroke();
                        ctx.restore();

                        // Text + badge giữa
                        const title = 'Nguy cơ gen', badge = levelCfg.label;
                        ctx.save();
                        ctx.textAlign = 'center'; ctx.textBaseline = 'middle';
                        ctx.fillStyle = '#324650'; ctx.font = '700 14px system-ui, Segoe UI, Roboto, Arial';
                        ctx.fillText(title, cx, cy - 6);

                        ctx.font = '600 12px system-ui, Segoe UI, Roboto, Arial';
                        const tw = ctx.measureText(badge).width, padX = 10, h = 22, w = tw + padX * 2, r = 10;
                        const bx = cx - w / 2, by = cy + 14;
                        const grad = ctx.createLinearGradient(0, by, 0, by + h); grad.addColorStop(0, levelCfg.badgeFrom); grad.addColorStop(1, levelCfg.badgeTo);

                        roundRect(ctx, bx, by, w, h, r);
                        ctx.fillStyle = grad; ctx.strokeStyle = 'rgba(255,255,255,0.7)'; ctx.lineWidth = 1; ctx.fill(); ctx.stroke();
                        ctx.fillStyle = '#fff'; ctx.fillText(badge, cx, by + h / 2 + 1);
                        ctx.restore();

                        function roundRect(c, x, y, w, h, r) {
                            r = Math.min(r, w / 2, h / 2);
                            c.beginPath();
                            c.moveTo(x + r, y);
                            c.arcTo(x + w, y, x + w, y + h, r);
                            c.arcTo(x + w, y + h, x, y + h, r);
                            c.arcTo(x, y + h, x, y, r);
                            c.arcTo(x, y, x + w, y, r);
                            c.closePath();
                        }
                    }
                };

                const canvas = document.getElementById('genRiskDonut');
                window.genRiskChart = new Chart(canvas, {
                    type: 'doughnut',
                    data: {
                        labels: ['Cao', 'Trung bình', 'Tốt'],
                        datasets: [{
                            data: OUTER_SEGMENTS,
                            backgroundColor: colors,
                            borderColor: colors,
                            borderAlign: 'inner',
                            borderWidth: 0,             // tắt viền dataset để viền trắng đều
                            spacing: 0,                 // không khe hở
                            hoverOffset: 0
                        }]
                    },
                    options: {
                        devicePixelRatio: Math.max(2, window.devicePixelRatio || 1),
                        responsive: true,            // dùng kích thước cố định ở canvas
                        maintainAspectRatio: true,
                        cutout: '75%',               // cutout phù hợp với kích thước 300px
                        rotation: -180,
                        circumference: 360,
                        animation: { duration: 0 },
                        plugins: { legend: { display: false }, tooltip: { enabled: false } },
                        layout: {
                            padding: 5              // tăng padding cho kích thước 300px
                        }
                    },
                    plugins: [geneticRiskPlugin]
                });

                // API công khai: gọi từ UI để đổi mức động
                window.setRiskLevel = function (level) {
                    if (!['red', 'orange', 'green'].includes(level)) return;
                    CURRENT_RISK_LEVEL = level;
                    // reset animation và kích hoạt vẽ lại
                    pointerAnimStart = null;
                    targetPointerRad = null;
                    if (window.genRiskChart) window.genRiskChart.update('none');
                };
            },
            initChartNextGeneticRisk: function () {
                const maxPercent = 24;
                const thresholds = { greenEnd: 8, orangeEnd: 16 }; // 0..8..16..24
                const markers = [
                    { value: 7, label: '7', type: 1001 },
                    { value: 11, label: '11', type: 1002 },
                ];

                const segmentedBar = {
                    id: 'segmentedBar',
                    beforeDatasetsDraw(chart) {
                        const { ctx, chartArea: { left, right, top, bottom } } = chart;
                        const xScale = chart.scales.x;
                        const yCenter = (top + bottom) / 2;
                        const barH = 32;                   // chiều cao thanh
                        const r = 12;                      // bo góc 2 đầu
                        const x0 = xScale.getPixelForValue(0);
                        const x1 = xScale.getPixelForValue(thresholds.greenEnd);
                        const x2 = xScale.getPixelForValue(thresholds.orangeEnd);
                        const x3 = xScale.getPixelForValue(maxPercent);

                        ctx.save();

                        // helper vẽ rounded rect chuẩn
                        const createAngleGradient = (x, y, w, h, angleDeg, stops) => {
                            // angle 0 -> left to right. Convert to radians
                            const a = (angleDeg % 360) * Math.PI / 180;
                            // effective length across the rect on that angle
                            const L = Math.abs(w * Math.cos(a)) + Math.abs(h * Math.sin(a));
                            const cx = x + w / 2, cy = y + h / 2;
                            const dx = Math.cos(a) * L / 2;
                            const dy = Math.sin(a) * L / 2;
                            const g = ctx.createLinearGradient(cx - dx, cy - dy, cx + dx, cy + dy);
                            g.addColorStop(0, stops[0]);
                            g.addColorStop(1, stops[1]);
                            return g;
                        };

                        const drawSeg = (xStart, xEnd, color, roundLeft, roundRight) => {
                            const w = Math.max(1, xEnd - xStart);
                            const rrL = roundLeft ? r : 0, rrR = roundRight ? r : 0;
                            const y = yCenter - barH / 2;
                            ctx.beginPath();
                            ctx.moveTo(xStart + rrL, y);
                            ctx.lineTo(xEnd - rrR, y);
                            ctx.quadraticCurveTo(xEnd, y, xEnd, y + rrR);
                            ctx.lineTo(xEnd, y + barH - rrR);
                            ctx.quadraticCurveTo(xEnd, y + barH, xEnd - rrR, y + barH);
                            ctx.lineTo(xStart + rrL, y + barH);
                            ctx.quadraticCurveTo(xStart, y + barH, xStart, y + barH - rrL);
                            ctx.lineTo(xStart, y + rrL);
                            ctx.quadraticCurveTo(xStart, y, xStart + rrL, y);
                            ctx.closePath();
                            // Hỗ trợ gradient: nếu color là mảng [start, end] hoặc object {angle, colors}
                            if (Array.isArray(color)) {
                                ctx.fillStyle = createAngleGradient(xStart, y, w, barH, 127, color);
                            } else if (typeof color === 'object' && color.colors) {
                                ctx.fillStyle = createAngleGradient(xStart, y, w, barH, color.angle ?? 127, color.colors);
                            } else {
                                ctx.fillStyle = color;
                            }
                            ctx.fill();
                        };

                        // 3 đoạn with perfect joins
                        drawSeg(x0, x1, { angle: 127, colors: ['#3D9C6F', '#69EC9F'] }, true, false);
                        drawSeg(x1, x2, { angle: 127, colors: ['#FEAC52', '#F95638'] }, false, false);
                        drawSeg(x2, x3, { angle: 127, colors: ['#EB5757', '#E14D4D'] }, false, true);

                        // Label trung tâm từng đoạn
                        ctx.font = '600 12px system-ui, Segoe UI, Roboto, Arial';
                        ctx.fillStyle = '#fff';
                        ctx.textAlign = 'center'; ctx.textBaseline = 'middle';
                        ctx.fillText('Trung Bình', (x0 + x1) / 2, yCenter);
                        ctx.fillText('Cao', (x1 + x2) / 2, yCenter);
                        ctx.fillText('Rất cao', (x2 + x3) / 2, yCenter);

                        // Markers (chuẩn hóa vị trí để không lệch)
                        const clamp = (v, min, max) => Math.max(min, Math.min(max, v));
                        const halfPixel = v => Math.round(v) + 0.5;
                        const y = Math.round(yCenter) + 0.5;
                        const barTopY = yCenter - barH / 2;

                        // Vẽ pin SVG có gradient theo hai màu
                        const drawSvgPin = (ctx, cx, cy, startColor, endColor, text) => {
                            // Path từ SVG (viewBox 0 0 30 36)
                            const pinPath = new Path2D("M9.25684 1.14258C12.0005 0.00547218 15.02 -0.291576 17.9326 0.289062C20.8456 0.869857 23.5212 2.30257 25.6191 4.40527C28.4252 7.22692 30 11.045 30 15.0244C29.9999 19.0036 28.425 22.821 25.6191 25.6426L15.6279 35.6338C15.5455 35.7162 15.4475 35.7816 15.3398 35.8262C15.2321 35.8708 15.1166 35.8945 15 35.8945C14.8834 35.8945 14.7679 35.8708 14.6602 35.8262C14.5525 35.7816 14.4545 35.7162 14.3721 35.6338L4.38086 25.6426C2.98492 24.2503 1.87894 22.595 1.12695 20.7725C0.374936 18.9498 -0.00804686 16.9961 0 15.0244C-0.00480055 12.0541 0.872598 9.14896 2.52051 6.67773C4.16841 4.20661 6.513 2.27987 9.25684 1.14258Z");
                            ctx.save();
                            // Đặt gốc sao cho đỉnh nhọn chạm gần mép trên của thanh
                            const tipSvgY = 35.8945;        // toạ độ tip trong viewBox 36
                            const px = cx - 15;             // 15 = 30/2
                            const tipTargetY = Math.round(barTopY - 2) + 0.5; // cách mép trên 2px
                            const py = tipTargetY - tipSvgY;
                            ctx.translate(px, py);

                            // Gradient chéo giống SVG
                            const grad = ctx.createLinearGradient(30, 11.334, 4.83206, 26.1688);
                            grad.addColorStop(0, startColor);
                            grad.addColorStop(1, endColor);
                            ctx.fillStyle = grad;
                            ctx.fill(pinPath);

                            // Viền trắng mỏng
                            ctx.strokeStyle = 'rgba(255,255,255,0.9)';
                            ctx.lineWidth = 1.2;
                            ctx.stroke(pinPath);
                            ctx.restore();

                            // Vẽ số ở giữa pin (căn giữa chuẩn theo bulb)
                            ctx.save();
                            const textX = Math.round(cx) + 0.5;
                            const textY = Math.round(py + 16) + 0.5; // 16 ~ tâm bulb trong hệ toạ độ hiện tại
                            ctx.fillStyle = '#fff';
                            ctx.font = '700 12px system-ui, Segoe UI, Roboto, Arial';
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'middle';
                            ctx.fillText(String(text), textX, textY);
                            ctx.restore();
                        };

                        const getColorByValue = (value) => {
                            // Xác định màu dựa trên giá trị value
                            if (value >= 0 && value < thresholds.greenEnd) {
                                return ['#2F80ED', '#56CCF2']; // Xanh: 0-8
                            } else if (value >= thresholds.greenEnd && value < thresholds.orangeEnd) {
                                return ['#FEAC52', '#F95638']; // Vàng: 8-16
                            } else if (value >= thresholds.orangeEnd && value <= maxPercent) {
                                return ['#EB5757', '#E14D4D']; // Đỏ: 16-24
                            }
                            return ['#6B7280', '#6B7280']; // Mặc định: xám
                        };
                        const getColorByType = (type) => {
                            // Xác định màu dựa trên type
                            if (type === 1001) {
                                return ['#2F80ED', '#56CCF2']; // Xanh
                            } else if (type === 1002) {
                                return ['#FEAC52', '#F95638']; // Vàng
                            }
                            return ['#6B7280', '#6B7280']; // Mặc định: xám
                        };
                        markers.forEach(m => {
                            const v = clamp(Number(m.value) || 0, 0, maxPercent);
                            let x = xScale.getPixelForValue(v);
                            // giữ khoảng cách an toàn với mép bo góc
                            const marginPx = 12;
                            const xMin = xScale.getPixelForValue(0) + marginPx;
                            const xMax = xScale.getPixelForValue(maxPercent) - marginPx;
                            x = clamp(x, xMin, xMax);
                            x = halfPixel(x);

                            // vẽ pin SVG với gradient theo type
                            const [startCol, endCol] = getColorByType(m.type);
                            drawSvgPin(ctx, x, y, startCol, endCol, String(m.label ?? v));
                        });

                        ctx.restore();
                        // Tiêu đề dưới trục (bám đáy trục x, tránh chèn vào bar/ticks)
                        const xAxis = chart.scales.x;
                        ctx.font = '600 12px system-ui, Segoe UI, Roboto, Arial';
                        ctx.fillStyle = '#6B7280';
                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'top';
                        ctx.fillText('Nguy cơ phát triển bệnh theo %', (left + right) / 2, xAxis.bottom + 14);
                    }
                };

                // Chart “rỗng” chỉ để lấy trục/ticks; nền vẽ bởi plugin
                const ctx = document.getElementById('riskIndexBar');
                new Chart(ctx, {
                    type: 'scatter',
                    data: { datasets: [] },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        layout: { padding: { top: 20, bottom: 28 } },
                        scales: {
                            x: {
                                type: 'linear', min: 0, max: maxPercent,
                                grid: { color: 'rgba(0,0,0,0.08)' },
                                ticks: { stepSize: 2, color: '#6B7280', padding: 10 },
                                border: { display: false }
                            },
                            y: {
                                min: 0, max: 1, display: false, grid: { display: false }, border: { display: false }
                            }
                        },
                        plugins: { legend: { display: false }, tooltip: { enabled: false } }
                    },
                    plugins: [segmentedBar]
                });

                // Hàm cập nhật legend - chỉ cập nhật giá trị
                const updateLegend = () => {
                    // Tìm điểm xanh và vàng từ markers dựa trên type
                    const greenMarker = markers.find(m => m.type === 1001);
                    const yellowMarker = markers.find(m => m.type === 1002);

                    // Cập nhật giá trị xanh
                    const greenValueEl = document.getElementById('greenValue');
                    if (greenValueEl && greenMarker) {
                        greenValueEl.textContent = greenMarker.label;
                    }

                    // Cập nhật giá trị vàng
                    const yellowValueEl = document.getElementById('yellowValue');
                    if (yellowValueEl && yellowMarker) {
                        yellowValueEl.textContent = yellowMarker.label;
                    }
                };
                const getColorByType = (type) => {
                    // Xác định màu dựa trên type
                    if (type === 1001) {
                        return ['#2F80ED', '#56CCF2']; // Xanh
                    } else if (type === 1002) {
                        return ['#FEAC52', '#F95638']; // Vàng
                    }
                    return ['#6B7280', '#6B7280']; // Mặc định: xám
                };
                // Cập nhật legend lần đầu
                updateLegend();

                // API cập nhật động
                window.setRiskBar = (thr, newMarkers) => {
                    if (thr?.greenEnd != null && thr?.orangeEnd != null) {
                        thresholds.greenEnd = thr.greenEnd;
                        thresholds.orangeEnd = thr.orangeEnd;
                    }
                    if (Array.isArray(newMarkers)) { 
                        markers.length = 0; 
                        newMarkers.forEach(m => markers.push(m)); 
                        updateLegend(); // Cập nhật legend khi thay đổi markers
                    }
                    Chart.getChart(ctx).update('none');
                };
            },
            initChartRiskOfDevelopingDisease: function (riskTrendData, riskTrendMarkerData) {
                const canvas = document.getElementById('riskOfDevelopingDisease');
                if (!canvas) {
                    console.error('Canvas riskOfDevelopingDisease not found');
                    return;
                }
                console.log('Initializing risk of developing disease chart...');

                // Use API data if available, otherwise use default demo data
                let chartData;
                if (riskTrendData && riskTrendData.length > 0) {
                    // Process API data
                    const displayAges = [20, 30, 40, 50, 60, 70];
                    chartData = {
                        displayAges: displayAges,
                        data: riskTrendData.map(item => ({
                            age: item.age,
                            currentRisk: item.currentRisk || 0,
                            lifestyleRisk: item.lifestyleRisk || 0,
                            averageRisk: item.averageRisk || 0
                        })),
                        userMarker: riskTrendMarkerData || { age: 23 }
                    };
                } else {
                    // Default demo data
                    chartData = {
                        displayAges: [20, 30, 40, 50, 60, 70],
                        data: [
                            { age: 20, currentRisk: 0.5, lifestyleRisk: 0.3, averageRisk: 0.2 },
                            { age: 21, currentRisk: 0.8, lifestyleRisk: 0.5, averageRisk: 0.35 },
                            { age: 22, currentRisk: 1.1, lifestyleRisk: 0.7, averageRisk: 0.5 },
                            { age: 23, currentRisk: 1.4, lifestyleRisk: 0.9, averageRisk: 0.65 },
                            { age: 24, currentRisk: 1.7, lifestyleRisk: 1.1, averageRisk: 0.8 },
                            { age: 25, currentRisk: 2.0, lifestyleRisk: 1.3, averageRisk: 0.95 },
                            { age: 26, currentRisk: 2.3, lifestyleRisk: 1.5, averageRisk: 1.1 },
                            { age: 27, currentRisk: 2.6, lifestyleRisk: 1.7, averageRisk: 1.25 },
                            { age: 28, currentRisk: 2.9, lifestyleRisk: 1.9, averageRisk: 1.4 },
                            { age: 29, currentRisk: 3.2, lifestyleRisk: 2.1, averageRisk: 1.55 },
                            { age: 30, currentRisk: 4.0, lifestyleRisk: 2.8, averageRisk: 2.0 },
                            { age: 31, currentRisk: 4.8, lifestyleRisk: 3.4, averageRisk: 2.4 },
                            { age: 32, currentRisk: 5.6, lifestyleRisk: 4.0, averageRisk: 2.8 },
                            { age: 33, currentRisk: 6.4, lifestyleRisk: 4.6, averageRisk: 3.2 },
                            { age: 34, currentRisk: 7.2, lifestyleRisk: 5.2, averageRisk: 3.6 },
                            { age: 35, currentRisk: 8.0, lifestyleRisk: 5.8, averageRisk: 4.0 },
                            { age: 36, currentRisk: 8.8, lifestyleRisk: 6.4, averageRisk: 4.4 },
                            { age: 37, currentRisk: 9.6, lifestyleRisk: 7.0, averageRisk: 4.8 },
                            { age: 38, currentRisk: 10.4, lifestyleRisk: 7.6, averageRisk: 5.2 },
                            { age: 39, currentRisk: 11.2, lifestyleRisk: 8.2, averageRisk: 5.6 },
                            { age: 40, currentRisk: 12.0, lifestyleRisk: 8.8, averageRisk: 6.0 },
                            { age: 41, currentRisk: 12.9, lifestyleRisk: 9.5, averageRisk: 6.5 },
                            { age: 42, currentRisk: 13.8, lifestyleRisk: 10.2, averageRisk: 7.0 },
                            { age: 43, currentRisk: 14.7, lifestyleRisk: 10.9, averageRisk: 7.5 },
                            { age: 44, currentRisk: 15.6, lifestyleRisk: 11.6, averageRisk: 8.0 },
                            { age: 45, currentRisk: 16.5, lifestyleRisk: 12.3, averageRisk: 8.5 },
                            { age: 46, currentRisk: 17.4, lifestyleRisk: 13.0, averageRisk: 9.0 },
                            { age: 47, currentRisk: 18.3, lifestyleRisk: 13.7, averageRisk: 9.5 },
                            { age: 48, currentRisk: 19.2, lifestyleRisk: 14.4, averageRisk: 10.0 },
                            { age: 49, currentRisk: 20.1, lifestyleRisk: 15.1, averageRisk: 10.5 },
                            { age: 50, currentRisk: 21.0, lifestyleRisk: 15.8, averageRisk: 11.0 },
                            { age: 51, currentRisk: 21.4, lifestyleRisk: 16.1, averageRisk: 11.25 },
                            { age: 52, currentRisk: 21.8, lifestyleRisk: 16.4, averageRisk: 11.5 },
                            { age: 53, currentRisk: 22.2, lifestyleRisk: 16.7, averageRisk: 11.75 },
                            { age: 54, currentRisk: 22.6, lifestyleRisk: 17.0, averageRisk: 12.0 },
                            { age: 55, currentRisk: 23.0, lifestyleRisk: 17.3, averageRisk: 12.25 },
                            { age: 56, currentRisk: 23.4, lifestyleRisk: 17.6, averageRisk: 12.5 },
                            { age: 57, currentRisk: 23.8, lifestyleRisk: 17.9, averageRisk: 12.75 },
                            { age: 58, currentRisk: 24.2, lifestyleRisk: 18.2, averageRisk: 13.0 },
                            { age: 59, currentRisk: 24.6, lifestyleRisk: 18.5, averageRisk: 13.25 },
                            { age: 60, currentRisk: 25.0, lifestyleRisk: 18.8, averageRisk: 13.5 },
                            { age: 61, currentRisk: 25.4, lifestyleRisk: 19.1, averageRisk: 13.75 },
                            { age: 62, currentRisk: 25.8, lifestyleRisk: 19.4, averageRisk: 14.0 },
                            { age: 63, currentRisk: 26.2, lifestyleRisk: 19.7, averageRisk: 14.25 },
                            { age: 64, currentRisk: 26.6, lifestyleRisk: 20.0, averageRisk: 14.5 },
                            { age: 65, currentRisk: 27.0, lifestyleRisk: 20.3, averageRisk: 14.75 },
                            { age: 66, currentRisk: 27.4, lifestyleRisk: 20.6, averageRisk: 15.0 },
                            { age: 67, currentRisk: 27.8, lifestyleRisk: 20.9, averageRisk: 15.25 },
                            { age: 68, currentRisk: 28.2, lifestyleRisk: 21.2, averageRisk: 15.5 },
                            { age: 69, currentRisk: 28.6, lifestyleRisk: 21.5, averageRisk: 15.75 },
                            { age: 70, currentRisk: 29.0, lifestyleRisk: 21.8, averageRisk: 16.0 }
                        ],
                        userMarker: {
                            age: 43,
                        }
                    };
                }

                const ages = chartData.data.map(item => item.age);
                const displayAges = chartData.displayAges;
                const currentRisk = chartData.data.map(item => item.currentRisk);
                const lifestyleRisk = chartData.data.map(item => item.lifestyleRisk);
                const averageRisk = chartData.data.map(item => item.averageRisk);
                
                // Tính max động từ dữ liệu
                const allValues = [...currentRisk, ...lifestyleRisk, ...averageRisk];
                const dataMax = Math.max(...allValues);
                const yAxisMax = Math.ceil(dataMax / 5) * 5 + 5; // Làm tròn lên và thêm 5% buffer

                // Điểm đánh dấu người dùng - lấy từ đường cam (currentRisk)
                const userAge = chartData.userMarker.age;
                const userRiskIndex = ages.findIndex(a => a === userAge);
                const userRisk = userRiskIndex >= 0 ? currentRisk[userRiskIndex] : (chartData.userMarker.risk || currentRisk[0]);

                // Store user marker data in window object for plugin access
                window.userMarkerData = {
                    age: userAge,
                    risk: userRisk
                };

                // Load ảnh người dùng trước
                window.userImage = new Image();
                window.userImage.crossOrigin = 'anonymous';
                // Use avatar from profile data if available
                const userAvatarSrc = (profileData && profileData.profile && profileData.profile.avatar) 
                    ? profileData.profile.avatar 
                    : '/images/userdefault.jpg';
                window.userImage.src = userAvatarSrc;

                try {
                    window.riskTrendChart = new Chart(canvas, {
                    type: 'line',
                    data: {
                        labels: ages,
                        datasets: [
                            {
                                label: 'Nguy cơ hiện tại',
                                data: currentRisk,
                                borderColor: function(context) {
                                    const chart = context.chart;
                                    const {ctx, chartArea} = chart;
                                    if (!chartArea) return '#FF8A65';
                                    
                                    const gradient = ctx.createLinearGradient(0, chartArea.top, 0, chartArea.bottom);
                                    gradient.addColorStop(0, '#FF8A65');
                                    gradient.addColorStop(1, '#FF7043');
                                    return gradient;
                                },
                                backgroundColor: 'rgba(255, 138, 101, 0.1)',
                                borderWidth: 3,
                                fill: false,
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 6
                            },
                            {
                                label: 'Sau thay đổi lối sống',
                                data: lifestyleRisk,
                                borderColor: function(context) {
                                    const chart = context.chart;
                                    const {ctx, chartArea} = chart;
                                    if (!chartArea) return '#FFB74D';
                                    
                                    const gradient = ctx.createLinearGradient(0, chartArea.top, 0, chartArea.bottom);
                                    gradient.addColorStop(0, '#FFB74D');
                                    gradient.addColorStop(1, '#FF9800');
                                    return gradient;
                                },
                                backgroundColor: 'rgba(255, 183, 77, 0.05)',
                                borderWidth: 2,
                                borderDash: [8, 4],
                                fill: false,
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 6
                            },
                            {
                                label: 'Trung bình quần thể',
                                data: averageRisk,
                                borderColor: function(context) {
                                    const chart = context.chart;
                                    const {ctx, chartArea} = chart;
                                    if (!chartArea) return '#1976D2';
                                    
                                    const gradient = ctx.createLinearGradient(0, chartArea.top, 0, chartArea.bottom);
                                    gradient.addColorStop(0, '#1976D2');
                                    gradient.addColorStop(1, '#0D47A1');
                                    return gradient;
                                },
                                backgroundColor: 'rgba(25, 118, 210, 0.1)',
                                borderWidth: 2,
                                fill: false,
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 6
                            },
                            // Vùng màu xanh với gradient (dưới đường trung bình)
                            {
                                label: 'Vùng dưới trung bình',
                                data: averageRisk,
                                borderColor: 'transparent',
                                backgroundColor: function(context) {
                                    const chart = context.chart;
                                    const {ctx, chartArea} = chart;
                                    if (!chartArea) return 'rgba(80, 159, 227, 0.15)';
                                    
                                    // Tạo gradient từ trái (đậm) sang phải (nhạt)
                                    const gradient = ctx.createLinearGradient(chartArea.left, 0, chartArea.right, 0);
                                    gradient.addColorStop(0, '#90CAF9'); // Đậm ở bên trái (gần trục Y)
                                    gradient.addColorStop(1, 'rgba(144, 202, 249, 0.00)'); // Nhạt ở bên phải
                                    return gradient;
                                },
                                fill: 'origin',
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 0
                            },
                            // Vùng màu xám (giữa đường cam nét đứt và đường xanh)
                            {
                                label: 'Vùng giữa',
                                data: averageRisk,
                                borderColor: 'transparent',
                                backgroundColor: 'rgba(128, 128, 128, 0.1)',
                                fill: '+1',
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 0
                            },
                            // Vùng màu cam với gradient (giữa đường cam liền nét và đường cam nét đứt)
                            {
                                label: 'Vùng cam',
                                data: currentRisk,
                                borderColor: 'transparent',
                                backgroundColor: function(context) {
                                    const chart = context.chart;
                                    const {ctx, chartArea} = chart;
                                    if (!chartArea) return 'rgba(254, 172, 82, 0.2)';
                                    
                                    // Tạo gradient với góc 72 độ
                                    const angle = 72 * Math.PI / 180; // Convert to radians
                                    const width = chartArea.right - chartArea.left;
                                    const height = chartArea.bottom - chartArea.top;
                                    
                                    // Tính toán điểm bắt đầu và kết thúc của gradient
                                    const x1 = chartArea.left + width * 0.5;
                                    const y1 = chartArea.top + height * 0.5;
                                    const length = Math.sqrt(width * width + height * height) * 0.6;
                                    
                                    const x2 = x1 + Math.cos(angle) * length;
                                    const y2 = y1 + Math.sin(angle) * length;
                                    
                                    const gradient = ctx.createLinearGradient(x1, y1, x2, y2);
                                    gradient.addColorStop(0, 'rgba(255, 183, 77, 0.3)');
                                    gradient.addColorStop(1, 'rgba(255, 183, 77, 0.00)');
                                    return gradient;
                                },
                                fill: '+1',
                                tension: 0,
                                pointRadius: 0,
                                pointHoverRadius: 0
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        interaction: {
                            intersect: false,
                            mode: 'index'
                        },
                        plugins: {
                            legend: {
                                display: false // Ẩn legend vì đã có legend HTML
                            },
                            tooltip: {
                                backgroundColor: 'rgba(0, 0, 0, 0.8)',
                                titleColor: '#fff',
                                bodyColor: '#fff',
                                borderColor: '#FEAC52',
                                borderWidth: 1,
                                callbacks: {
                                    title: function(context) {
                                        return `Tuổi: ${context[0].label}`;
                                    },
                                    label: function(context) {
                                        return `${context.dataset.label}: ${context.parsed.y.toFixed(1)}%`;
                                    }
                                }
                            }
                        },
                        scales: {
                            x: {
                                type: 'linear',
                                min: 20,
                                max: 70,
                                title: {
                                    display: true,
                                    text: 'Tuổi',
                                    font: {
                                        size: 12,
                                        weight: 'bold'
                                    },
                                    color: '#6B7280'
                                },
                                grid: {
                                    color: 'rgba(0, 0, 0, 0.05)',
                                    drawBorder: false
                                },
                                ticks: {
                                    color: '#6B7280',
                                    stepSize: 10,
                                    callback: function(value) {
                                        return displayAges.includes(value) ? value : '';
                                    }
                                }
                            },
                            y: {
                                title: {
                                    display: true,
                                    text: 'Nguy cơ phát triển bệnh: %',
                                    font: {
                                        size: 12,
                                        weight: 'bold'
                                    },
                                    color: '#6B7280'
                                },
                                min: 0,
                                max: yAxisMax,
                                grid: {
                                    color: 'rgba(0, 0, 0, 0.05)',
                                    drawBorder: false
                                },
                                ticks: {
                                    color: '#6B7280',
                                    stepSize: 5,
                                    maxTicksLimit: 8,
                                    callback: function(value) {
                                        return value + '%';
                                    }
                                }
                            }
                        },
                        elements: {
                            point: {
                                hoverBackgroundColor: '#FEAC52',
                                hoverBorderColor: '#fff',
                                hoverBorderWidth: 2
                            }
                        }
                    },
                    plugins: [{
                        id: 'userMarker',
                        afterDatasetsDraw(chart) {
                            const { ctx, chartArea: { left, right, top, bottom } } = chart;
                            const xScale = chart.scales.x;
                            const yScale = chart.scales.y;
                            
                            // Get user marker data from window object
                            const markerAge = window.userMarkerData?.age || userAge;
                            const markerRisk = window.userMarkerData?.risk || userRisk;
                            
                            // Vị trí điểm người dùng
                            const x = xScale.getPixelForValue(markerAge);
                            const y = yScale.getPixelForValue(markerRisk);
                            
                            
                            // Vẽ SVG pin custom
                            const pinSize = 30;
                            const pinPath = new Path2D("M9.25684 1.14355C12.0007 0.00624169 15.0207 -0.290753 17.9336 0.290039C20.8465 0.870825 23.5213 2.3036 25.6191 4.40625C28.4253 7.22791 30.001 11.0459 30.001 15.0254C30.0009 19.0047 28.4252 22.822 25.6191 25.6436L15.6279 35.6348C15.5455 35.7172 15.4475 35.7825 15.3398 35.8271C15.2323 35.8716 15.1173 35.8954 15.001 35.8955C14.8845 35.8955 14.7688 35.8717 14.6611 35.8271C14.5534 35.7825 14.4555 35.7172 14.373 35.6348L4.38184 25.6436C2.98576 24.2512 1.87899 22.5961 1.12695 20.7734C0.374962 18.9508 -0.008045 16.997 0 15.0254C-0.00480053 12.0551 0.872612 9.14993 2.52051 6.67871C4.16842 4.20749 6.51293 2.28088 9.25684 1.14355Z");
                            
                            // Scale và position pin - mũi tên trùng với điểm trên đường
                            const scale = pinSize / 30; // SVG viewBox là 30x36
                            const pinX = x - 15 * scale; // Center pin theo X
                            const pinY = y - 35.8945 * scale; // Mũi tên (tip) trùng với điểm trên đường
                            
                            ctx.save();
                            ctx.translate(pinX, pinY);
                            ctx.scale(scale, scale);
                            
                            // Gradient fill
                            const gradient = ctx.createLinearGradient(30.0011, 11.3349, 4.83263, 26.1705);
                            gradient.addColorStop(0, '#FFA726');
                            gradient.addColorStop(1, '#FF8A65');
                            ctx.fillStyle = gradient;
                            ctx.fill(pinPath);
                            
                            // White border
                            ctx.strokeStyle = 'white';
                            ctx.lineWidth = 1.5;
                            ctx.stroke(pinPath);
                            
                            // Vẽ ảnh hình tròn ở giữa pin (bulb area)
                            const imageSize = 22; // Kích thước ảnh lớn hơn để vừa vặn
                            
                            // Kiểm tra xem ảnh đã load chưa
                            if (window.userImage && window.userImage.complete) {
                                // Vẽ ảnh hình tròn
                                ctx.save();
                                ctx.beginPath();
                                ctx.arc(15, 15, imageSize/2, 0, Math.PI * 2);
                                ctx.clip();
                                
                                // Vẽ ảnh
                                ctx.drawImage(window.userImage, 15 - imageSize/2, 15 - imageSize/2, imageSize, imageSize);
                                
                                ctx.restore();
                                
                                // Viền cho ảnh
                                ctx.strokeStyle = '#FEAC52';
                                ctx.lineWidth = 1;
                                ctx.beginPath();
                                ctx.arc(15, 15, imageSize/2, 0, Math.PI * 2);
                                ctx.stroke();
                            } else {
                                // Fallback: vẽ icon nếu ảnh chưa load
                                ctx.fillStyle = '#fff';
                                ctx.beginPath();
                                ctx.arc(15, 15, imageSize/2, 0, Math.PI * 2);
                                ctx.fill();
                                
                                ctx.strokeStyle = '#FEAC52';
                                ctx.lineWidth = 1;
                                ctx.stroke();
                                
                                ctx.fillStyle = '#FEAC52';
                                ctx.font = '12px Arial';
                                ctx.textAlign = 'center';
                                ctx.textBaseline = 'middle';
                                ctx.fillText('👤', 15, 15);
                            }
                            
                            ctx.restore();
                        }
                    }]
                });
                } catch (error) {
                    console.error('Error creating chart:', error);
                }
            },
            init: function (apiData) {
                this.initChartGeneticRisk();
                this.initChartNextGeneticRisk();
                
                // Pass API data to risk trend chart if available
                const riskTrend = apiData?.riskTrend || null;
                const riskTrendMarker = apiData?.riskTrendMarker || null;
                this.initChartRiskOfDevelopingDisease(riskTrend, riskTrendMarker);
            }
        };
        window.chartCanvasInstance.init(apiData);
    }
    function init() {
        updateRiskPointerColor();
        // Load profile first, then render charts with data
        loadProfile();
        // Render charts with empty data first (will be updated when API returns)
        renderChart(null);
    }

    document.addEventListener('DOMContentLoaded', function () {
        init();
    });
})();
